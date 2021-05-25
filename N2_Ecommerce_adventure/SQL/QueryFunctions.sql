Use N2_dudu_Viotti_ecommerce;
Go

create Function fnc_CalculaValorPedido(@idPedido int) Returns money as
	begin
		declare @Quantidade int;
		declare @Desconto real;
		declare @Preco money;
		declare @Total money;
		declare @Auxiliar money;
		declare CursorItensPedido cursor for select Quantidade,Desconto,Preco from tbPedidosxProdutos where idPedido = @idPedido;
		open CursorItensPedido;
		set @Total=0.00;
		fetch next from CursorItensPedido into @Quantidade,@Desconto,@Preco;
		while @@FETCH_STATUS = 0 
			begin
				set @Auxiliar=@Quantidade*@Preco*(1-@Desconto);
				Set @Total=@Total+@Auxiliar;
				fetch next from CursorItensPedido into @Quantidade,@Desconto,@Preco;
			end
		close CursorItensPedido;
		deallocate CursorItensPedido;

		return @Total;
	end
Go
create Function fnc_GetAllPedidos( @idstatus int) returns 
@temporaria table(
		id int,
		data smalldatetime,		Nome varchar(20),		Cidade varchar(50),		CEP varchar(10),		Valor money		) as
	begin
		declare @Quantidade int;
		declare @Desconto real;
		declare @Preco money;
		declare @Total money;
		declare @Auxiliar money;
		declare @id int;
		declare @data smalldatetime;
		declare @Nome varchar(20);
		declare @Cidade varchar(50);		declare @CEP varchar(10);


		declare CursorPedidos cursor for select* from vw_Pedidos_Em_Aberto;
		Open CursorPedidos;
		fetch next from CursorPedidos into @id,@data,@Nome,@Cidade,@CEP;

		while @@FETCH_STATUS = 0 

			begin
				declare CursorItensPedido cursor for select Quantidade,Desconto,Preco from tbPedidosxProdutos where idPedido = @id;
				open CursorItensPedido;
				set @Total=0.00;
				fetch next from CursorItensPedido into @Quantidade,@Desconto,@Preco;
				while @@FETCH_STATUS = 0 
					
					begin
						set @Auxiliar=@Quantidade*@Preco*(1-(@Desconto/100));
						Set @Total=@Total+@Auxiliar;
						
						fetch next from CursorItensPedido into @Quantidade,@Desconto,@Preco;
					end
				close CursorItensPedido;
				deallocate CursorItensPedido;
				insert into @temporaria (id,data,Nome,Cidade,CEP,Valor) Values(@id,@data,@Nome,@Cidade,@CEP,@Total);
				fetch next from CursorPedidos into @id,@data,@Nome,@Cidade,@CEP;
			end
			close CursorPedidos;
			deallocate CursorPedidos;

		return ;
	end
GO
