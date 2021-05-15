create procedure spInsert_tbTipoUsuario(@id int, @Tipo varchar(20))as  --as SP podem receber parametros
begin
	insert into tbTipoUsuario(Tipo) 
	values (@Tipo)
end

create procedure spUpdate_tbTipoUsuario(@id int, @Tipo varchar(20))as
begin
	update tbTipoUsuario set
	Tipo=@Tipo
	where id=@id
end

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
create procedure spDelete_Usuario(@idUsuario int) as
begin
	update tbUsuario set
	statusUsuario =0  -- para garantir os dados adequados do pedido mesmo que o usuario seja deletado
	where id =@idUsuario
end
create procedure spVerificaUsuario(@login varchar(30),@senha  varchar(30)) as
begin 
	select * from tbUsuario where email=@login and senha=@senha
end


create procedure spConsultaEnderecosUsuario (@idUsuario int) as
begin
	select * from tbUsuarioxEnderecos 
	where id_usuario=@idUsuario
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

