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
