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


create function fnc_AllPedidos(@idstatus int) Returns money as
	begin
		declare @Quantidade int;
		declare @Desconto real;
		declare @Preco money;
		declare @Total money;
		declare @Auxiliar money;
		declare @idPedido int;

		declare CursorPedidos cursor for select id from tbPedidos where idStatus=@idstatus;
		open CursorPedidos;
		set @Total=0.00;
		fetch next from CursorPedidos into @idPedido;
		while @@FETCH_STATUS = 0 
			begin
				declare CursorItensPedido cursor for select Quantidade,Desconto,Preco from tbPedidosxProdutos where idPedido = @idPedido;
				open CursorItensPedido;
				fetch next from CursorItensPedido into @Quantidade,@Desconto,@Preco;
				while @@FETCH_STATUS = 0 
					begin
						set @Auxiliar=@Quantidade*@Preco*(1-(@Desconto/100));
						Set @Total=@Total+@Auxiliar;
						fetch next from CursorItensPedido into @Quantidade,@Desconto,@Preco;
					end
				close CursorItensPedido;
				deallocate CursorItensPedido;
			fetch next from CursorPedidos into @idPedido;
			end

		close CursorPedidos;
		deallocate CursorPedidos;
		return @Total;
	end
Go

