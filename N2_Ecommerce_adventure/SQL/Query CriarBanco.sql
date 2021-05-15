Create database N2_dudu_Viotti_ecommerce;

Use N2_dudu_Viotti_ecommerce;


create table tbTipoUsuario(
	id int identity(1,1) not null,
	Tipo varchar(20) not null unique,
	primary key(id)
);

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

create table tbEnderecos(
	id int identity(1,1) not null,
	Rua varchar(20) not null,
	Complemento varchar(20) not null,
	numero int not null,
	Cep varchar(10) not null,
	Cidade varchar(15) not null,
	primary key (id)
);

create table tbUsuarioxEnderecos(
	id_usuario int foreign key references tbUsuario (id),
	id_endereco int foreign key references tbEnderecos (id),
	primary key (id_usuario,id_endereco)
);

create table tbCategoriaProdutos(
	id int identity(1,1) not null,
	Categoria varchar(20) not null unique,
	primary key(id)
);

create table tbProdutos(
	id int identity(1,1) not null,
	Nome varchar(20) not null unique,
	Preco decimal(7,2) not null default 00000.00,
	Descricao varchar(100) not null,
	Foto varbinary(max),
	Quantidade int not null default 0,
	idCategoria int foreign key references tbCategoriaProdutos (id),
	primary key (id)
);

create table tbStatusPedido(
	id int identity(1,1) not null,
	PedidoStatus varchar(20) not null unique,
	primary key(id)
);

create table tbPedidos(
	id int identity (1,1) not null,
	idStatus int foreign key references tbStatusPedido (id) not null,
	idUsuario int not null,
	idEndereco int not null,
	foreign key (idUsuario,idEndereco) references tbUsuarioxEnderecos(id_usuario,id_endereco),
	data smalldatetime not null,
	primary key (id)
);

create table tbPedidosxProdutos(
	idPedido int foreign key references tbPedidos (id) not null,
	idProduto int foreign key references tbProdutos (id) not null,
	Quantidade int not null default 1,
	primary key (idPedido,idProduto)
);

insert into tbTipoUsuario (Tipo) values ('Normal');
insert into tbTipoUsuario (Tipo) values ('Gerente');

set dateformat dmy;
insert into tbUsuario (Nome,Nascimento,email,senha,cpf,idTipoUsuario) Values
('Administrador','15/05/2021','admin','admin','cpf',2);
