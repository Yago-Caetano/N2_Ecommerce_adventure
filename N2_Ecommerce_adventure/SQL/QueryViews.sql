Use N2_dudu_Viotti_ecommerce;
create view vw_Pedidos_Em_Aberto  as
	select P.id,P.data,U.Nome, E.Cidade,E.Cep from tbPedidos P inner join
	tbUsuario U on p.idUsuario=U.id inner join
	tbEnderecos E on P.idEndereco=E.id
	where P.idStatus=1

create view vw_Pedidos_Concluidos  as
	select P.id,P.data,U.Nome, E.Cidade,E.Cep from tbPedidos P inner join
	tbUsuario U on p.idUsuario=U.id inner join
	tbEnderecos E on P.idEndereco=E.id
	where P.idStatus=2

--criar view de estoque

select * from vw_Pedidos_Em_Aberto
