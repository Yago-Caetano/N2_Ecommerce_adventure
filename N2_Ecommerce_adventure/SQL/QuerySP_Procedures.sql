Use N2_dudu_Viotti_ecommerce;
go
-------------------------------------------------------------------------Tipos de usuario
create procedure spInsert_tbTipoUsuario(@id int, @Tipo varchar(20))as  --as SP podem receber parametros
begin
	insert into tbTipoUsuario(Tipo) 
	values (@Tipo)
end
GO
create procedure spUpdate_tbTipoUsuario(@id int, @Tipo varchar(20))as
begin
	update tbTipoUsuario set
	Tipo=@Tipo
	where id=@id
end
GO
--------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------Categoria de produtos
create procedure spInsert_tbCategoriaProdutos(@id int, @Categoria varchar(20))as  --as SP podem receber parametros
begin
	insert into tbCategoriaProdutos(Categoria) 
	values (@Categoria)
end
GO
create procedure spUpdate_tbCategoriaProdutos(@id int, @Categoria varchar(20))as
begin
	update tbCategoriaProdutos set
	Categoria=@Categoria
	where id=@id
end
GO
--------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------Produtos
create procedure spInsert_tbProdutos
(	@id int, 
	@Nome varchar(20),
	@Preco money ,
	@Descricao varchar(100),
	@Foto varbinary(max),
	@Quantidade int,
	@Desconto real,
	@idCategoria int
)as  --as SP podem receber parametros
begin
	
	insert into tbProdutos(Nome,Preco,Descricao,Foto,Quantidade,Desconto,idCategoria) 
	values (@Nome,@Preco,@Descricao,@Foto,@Quantidade,@Desconto,@idCategoria) 
end
GO
create procedure spUpdate_tbProdutos
(	@id int, 
	@Nome varchar(20),
	@Preco money ,
	@Descricao varchar(100),
	@Foto varbinary(max),
	@Quantidade int,
	@Desconto real,
	@idCategoria int
)as  --as SP podem receber parametros
begin
	update tbProdutos set
	Nome =@Nome,
	Preco =@Preco ,
	Descricao =@Descricao,
	Foto =@Foto,
	Quantidade=@Quantidade,
	Desconto=@Desconto,
	idCategoria=@idCategoria
	where id =@id

end
GO
create procedure spConsultaNomeProduto (@Id int) as
begin 
	select id,Nome from tbProdutos where id=@Id
end
GO
-----------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------Usuarios
create procedure spInsert_tbUsuario
(	@id int, 
	@Nome varchar(20),
	@Nascimento smalldatetime ,
	@email varchar(30),
	@senha  varchar(30),
	@cpf  varchar(20),
	@idTipoUsuario  int,
	@statusUsuario bit
)as  --as SP podem receber parametros
begin
	
	set dateformat dmy;
	insert into tbUsuario(Nome,Nascimento,email,senha,cpf,idTipoUsuario) 
	values (@Nome,@Nascimento,@email,@senha,@cpf,@idTipoUsuario) 
end
GO
create procedure spUpdate_tbUsuario
(	@id int, 
	@Nome varchar(20),
	@Nascimento smalldatetime ,
	@email varchar(30),
	@senha  varchar(30),
	@cpf  varchar(20),
	@idTipoUsuario  int,
	@statusUsuario bit
)as  --as SP podem receber parametros
begin
	set dateformat dmy;
	update tbUsuario set
	Nome=@Nome,
	Nascimento=@Nascimento,
	email=@email,
	senha=@senha,
	cpf=@cpf,
	idTipoUsuario=@idTipoUsuario
	where id =@id
end
GO
Create procedure spDelete_Usuario(@idUsuario int) as -- Usuarios n�o s�o realmente deletados, s� desativados
begin
	begin transaction

	declare @idPedido int;
	declare @idEndereco int;
	declare @idPedidoItem int;

	update tbUsuario set
	statusUsuario =0  -- para garantir os dados adequados do pedido mesmo que o usuario seja deletado
	where id =@idUsuario
	print 'Usuario desabilitado'

	declare cursorEndereco cursor for select id_endereco from tbUsuarioxEnderecos where id_usuario = @idUsuario;
	open cursorEndereco;
	fetch next from cursorEndereco into @idEndereco;
	while @@FETCH_STATUS = 0  
		begin
			update tbEnderecos set
			statusEnd =0  -- para garantir os dados adequados do pedido mesmo que o usuario seja deletado
			where id =@idEndereco
			print 'Endereço desabilitado'
			fetch next from cursorEndereco into @idEndereco;
		end
	close cursorEndereco;
	deallocate cursorEndereco;

	declare cursorPedido cursor for select id from tbPedidos where idUsuario = @idUsuario and idStatus=1;

	open cursorPedido;

	fetch next from cursorPedido into @idPedido

	while @@FETCH_STATUS = 0  
		begin
			print 'Pedido encontrado: ' + cast(@idPedido as varchar(5))
			declare cursorPedidoItem cursor for select idProduto from tbPedidosxProdutos where idPedido = @idPedido;
			open cursorPedidoItem;
			fetch next from cursorPedidoItem into @idPedidoItem;
			while  @@FETCH_STATUS = 0 
				begin
					execute spUpdate_tbPedidosxProdutos @idPedido,@idPedidoItem,0
					fetch next from cursorPedidoItem into @idPedidoItem;
				end
			close cursorPedidoItem;
			deallocate cursorPedidoItem;

			delete from tbPedidosxProdutos where idPedido=@idPedido;
		
			delete from tbPedidos where id=@idPedido;
			fetch next from cursorPedido into @idPedido;
		end
	close cursorPedido;
	deallocate cursorPedido;

	commit

end
GO
create procedure spVerificaUsuario(@login varchar(30),@senha  varchar(30)) as
begin 
	select * from tbUsuario where email=@login and senha=@senha and statusUsuario=1
end
GO
create procedure spConsultaDadosUsuario (@idUsuario int) as
begin
	select id,Nome,email,cpf from tbUsuario where id=@idUsuario
end
GO
create procedure spConsultaEnderecosUsuario (@idUsuario int) as
begin
	select tbE.id from tbUsuarioxEnderecos tbUE inner join tbEnderecos tbE on tbUE.id_endereco=tbE.id
	where tbUE.id_usuario=@idUsuario and tbE.statusEnd=1
end
GO
-----------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------Enderecos
create procedure spInsert_tbEnderecos
(	@id int, 
	@Rua varchar(20),
	@Complemento varchar(20),
	@numero int ,
	@CEP varchar(10),
	@Cidade varchar(15),
	@statusEnd BIT
)
as  --as SP podem receber parametros
begin
	insert into tbEnderecos(Rua,Complemento,numero,CEP,Cidade,statusEnd) 
	values (@Rua,@Complemento,@numero,@CEP,@Cidade,@statusEnd) 
end
GO
create procedure spUpdate_tbEnderecos
(	@id int, 
	@Rua varchar(20),
	@Complemento varchar(20),
	@numero int ,
	@CEP varchar(10),
	@Cidade varchar(15),
	@statusEnd BIT
)
as  --as SP podem receber parametros
begin
	update tbEnderecos set
	Rua=@Rua,
	Complemento=@Complemento,
	numero=@numero,
	CEP=@CEP,
	Cidade=@Cidade,
	statusEnd=@statusEnd
	where id=@id
end
GO
create procedure spDelete_Enderecos(@idEndereco int) as -- Enderecos n�o s�o realmente deletados, s� desativados
begin
	update tbEnderecos set
	statusEnd =0  -- para garantir os dados adequados do pedido mesmo que o usuario seja deletado
	where id =@idEndereco
end
GO
create procedure spInsertUserEndereco(@idUsuario int,@idEndereco int) as
begin
	insert into tbUsuarioxEnderecos (id_usuario,id_endereco) Values
	(@idUsuario,@idEndereco)
end
GO
---------------------------------------------------------------------------------------------Pedidos
create procedure spInsert_tbPedidos
(
	@id int,
	@idStatus int,
	@idUsuario int,
	@idEndereco int,
	@data smalldatetime
) as
begin
	insert into tbPedidos (idStatus,idUsuario,idEndereco,data) Values 
	(@idStatus,@idUsuario,@idEndereco,@data)

end
GO
create procedure spUpdate_tbPedidos
(
	@id int,
	@idStatus int,
	@idUsuario int,
	@idEndereco int,
	@data smalldatetime
) as
begin
	update tbPedidos set

	idStatus =@idStatus,
	idUsuario=@idUsuario,
	idEndereco=@idEndereco,
	data=@data
	where id=@id
end
GO

---------------------------------------------------------------------------------------------Pedido produto
create procedure spInsert_tbPedidosxProdutos
(
	@idPedido int ,
	@idProduto int ,
	@Quantidade int
)
as
begin
	insert into tbPedidosxProdutos (idPedido,idProduto,Quantidade) Values
	(@idPedido,@idProduto,@Quantidade)
end
GO
create procedure spUpdate_tbPedidosxProdutos
(
	@idPedido int ,
	@idProduto int ,
	@Quantidade int
)
as
begin
	update tbPedidosxProdutos set
	Quantidade= @Quantidade
	where idPedido =@idPedido and idProduto =@idProduto
end
GO

create procedure spDelete_tbPedidosxProdutos
(
	@idPedido int ,
	@idProduto int
)
as
begin
	execute spUpdate_tbPedidosxProdutos @idPedido,@idProduto,0
	delete tbPedidosxProdutos where idPedido =@idPedido and idProduto =@idProduto
end
GO
create procedure splistar_itensPedido
(
	@idPedido int
)
as begin 
	select * from tbPedidosxProdutos where idPedido=@idPedido
end
Go
create procedure spConsulta_tbPedidosxProdutos
(
	@idPedido int ,
	@idProduto int
)
as
begin
	select * from tbPedidosxProdutos where idPedido=@idPedido and idProduto=@idProduto
end
GO

---------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------Procedures genericas
create procedure spDelete
(
 @id int ,
 @tabela varchar(max)
)
as
begin
 declare @sql varchar(max);
 set @sql = ' delete ' + @tabela +
 ' where id = ' + cast(@id as varchar(max))
 exec(@sql)
end
GO

create procedure spConsulta
(
 @id int ,
 @tabela varchar(max)
)
as
begin
 declare @sql varchar(max);
 set @sql = 'select * from ' + @tabela +
 ' where id = ' + cast(@id as varchar(max))
 exec(@sql)
end
GO

create procedure spListagem
(
 @tabela varchar(max),
 @ordem varchar(max))
as
begin
 exec('select * from ' + @tabela +
 ' order by ' + @ordem)
end
GO
create procedure spProximoId
(@tabela varchar(max))
as
begin
 exec('select isnull(max(id) +1, 1) as MAIOR from '
 +@tabela)
end
GO
--------------------------------------------------------------------------------------------------