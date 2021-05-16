Create database N2_dudu_Viotti_ecommerce;
go
Use N2_dudu_Viotti_ecommerce;
go
create table tbTipoUsuario(
	id int identity(1,1) not null,
	Tipo varchar(20) not null unique,
	primary key(id)
);
GO
create table tbUsuario(
	id int identity(1,1) not null,
	Nome varchar(20) not null,
	Nascimento smalldatetime not null,
	email varchar(30) unique not null,
	senha varchar(30) not null,
	cpf varchar(20) not null unique,
	idTipoUsuario int foreign key references tbTipoUsuario(id),
	statusUsuario BIT default 1, -- indica se o usuario está ativo ou não
	primary key(id)
);
GO
create table tbEnderecos(
	id int identity(1,1) not null,
	Rua varchar(20) not null,
	Complemento varchar(20) not null,
	numero int not null,
	Cep varchar(10) not null,
	Cidade varchar(15) not null,
	statusEnd BIT default 1, -- indica se o endereço está ativo ou não
	primary key (id)
);
GO
create table tbUsuarioxEnderecos(
	id_usuario int foreign key references tbUsuario (id),
	id_endereco int foreign key references tbEnderecos (id),
	primary key (id_usuario,id_endereco)
);
GO
create table tbCategoriaProdutos(
	id int identity(1,1) not null,
	Categoria varchar(20) not null unique,
	primary key(id)
);
GO
create table tbProdutos(
	id int identity(1,1) not null,
	Nome varchar(20) not null unique,
	Preco money not null,
	Descricao varchar(100) not null,
	Foto varbinary(max),
	Quantidade int not null default 0,
	Desconto real default 0.00,
	QuantidadeEmOrdem int default 0,
	idCategoria int foreign key references tbCategoriaProdutos (id),
	primary key (id)
);
GO
create table tbStatusPedido(
	id int identity(1,1) not null,
	PedidoStatus varchar(20) not null unique,
	primary key(id)
);
GO
create table tbPedidos(
	id int identity (1,1) not null,
	idStatus int foreign key references tbStatusPedido (id) not null,
	idUsuario int not null,
	idEndereco int not null,
	foreign key (idUsuario,idEndereco) references tbUsuarioxEnderecos(id_usuario,id_endereco),
	data smalldatetime not null,
	primary key (id)
);
GO
create table tbPedidosxProdutos(
	idPedido int foreign key references tbPedidos (id) not null,
	idProduto int foreign key references tbProdutos (id) not null,
	Quantidade int not null default 1,
	Disconto real default 0.00 not null,
	Preco money not null,

	primary key (idPedido,idProduto)
);
GO
insert into tbTipoUsuario (Tipo) values ('Normal');
insert into tbTipoUsuario (Tipo) values ('Gerente');

insert into tbStatusPedido(PedidoStatus) Values ('Pendente');
insert into tbStatusPedido(PedidoStatus) Values ('Concluido');

set dateformat dmy;
insert into tbUsuario (Nome,Nascimento,email,senha,cpf,idTipoUsuario) Values
('Administrador','15/05/2021','admin','admin','cpf',2);
