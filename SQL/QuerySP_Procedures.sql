create procedure spInsert_jogos(@id int, @descricao varchar(max),
								   @valor_locacao decimal(5,2),@categoriaID as int, @data_aquisicao as smalldatetime) as  --as SP podem receber parametros
begin
	insert into jogos(id,descricao,valor_locacao,data_aquisicao,categoriaID) 
	values (@id,@descricao,@valor_locacao,@data_aquisicao,@categoriaID)
end


create procedure spUpdate_jogos(@id int, @descricao varchar(max),
								   @valor_locacao decimal(5,2),@categoriaID as int, @data_aquisicao as smalldatetime) as  --as SP podem receber parametros
begin
	update jogos set
	descricao=@descricao,
	valor_locacao=@valor_locacao,
	data_aquisicao=@data_aquisicao,
	categoriaID=@categoriaID
	where id=@id
end

create procedure spConsultaEnderecosUsuario (@idUsuario int) as
begin
	select * from tbUsuarioxEnderecos 
	where id_usuario=@idUsuario
end

create procedure spInsert_Categorias(@id int, @descricao nvarchar(max)) as  --as SP podem receber parametros
begin
	insert into Categorias(id,descricao) 
	values (@id,@descricao)
end
create procedure spInsert_Categorias(@id int, @descricao varchar(max)) as  --as SP podem receber parametros
begin
	update Categorias set
	descricao=@descricao
	where id=@id
end

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


create procedure spListagem
(
 @tabela varchar(max),
 @ordem varchar(max))
as
begin
 exec('select * from ' + @tabela +
 ' order by ' + @ordem)
end

create procedure spProximoId
(@tabela varchar(max))
as
begin
 exec('select isnull(max(id) +1, 1) as MAIOR from '
 +@tabela)
end
create procedure spListagem
(
 @tabela varchar(max),
 @ordem varchar(max))
as
begin
 exec('select * from ' + @tabela +
 ' order by ' + @ordem)
end