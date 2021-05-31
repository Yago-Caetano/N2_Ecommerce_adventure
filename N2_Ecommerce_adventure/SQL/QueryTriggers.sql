Use N2_dudu_Viotti_ecommerce;
go
create trigger trg_Delete_Pedido on tbPedidos instead of delete
as
begin
	print 'Entrou na trigger'
	declare @idPedido int;
	declare @idStatus int;
	declare @quantidadeAux int;
	declare @idProdutoAux int;

	set @idPedido = (select id from deleted);
	set @idStatus = (select idStatus from deleted);

	declare cursorItemPedido cursor for select idProduto,Quantidade from tbPedidosxProdutos where idPedido = @idPedido;


	/*
		Verifica se o pedido está em aberto a fim de corrigir o estoque
	*/
	if(@idStatus = 1)
	begin
		print 'Entrou no if'
		open cursorItemPedido;

		fetch next from cursorItemPedido into @idProdutoAux,@quantidadeAux;

		while @@FETCH_STATUS = 0  
			begin
				declare @qtdOrdem int;
				declare @qtdEstoque int;

				set @qtdEstoque = (select Quantidade from tbProdutos where id = @idProdutoAux);
				set @qtdOrdem = (select QuantidadeEmOrdem from tbProdutos where id = @idProdutoAux);

				 
				update tbProdutos set Quantidade = @qtdEstoque+@quantidadeAux, QuantidadeEmOrdem = @qtdOrdem-@quantidadeAux where id = @idProdutoAux;
				
				fetch next from cursorItemPedido into @idProdutoAux,@quantidadeAux;
			end
			close cursorItemPedido;
			deallocate cursorItemPedido;
	end

	/*
		executa os deletes
	*/
	
	delete from tbPedidosxProdutos where idPedido =@idPedido;
	

	
	delete from tbPedidos where id = @idPedido;
	
end
Go
--Confere os itens em estoque e se poss�vel insere junto com o pre�o e desconto do momento
create trigger trg_Insert_ItenPedido on tbPedidosxProdutos instead of insert as
begin
	declare @idPedido int
	declare @idProduto int 
	declare @Estoque int
	declare @Quantidade int
	declare @QuantidadeEmOrdem int
	declare @Preco money
	declare @Desconto real

	set @idPedido = (select idPedido from inserted);
	set @idProduto = (select idProduto from inserted);
	set @Quantidade = (select Quantidade from inserted)

	set @Estoque=(select Quantidade from tbProdutos where id=@idProduto)

	if @Estoque>=@Quantidade
		begin
			set @Preco=(select Preco from tbProdutos where id=@idProduto)
			set @QuantidadeEmOrdem=(select QuantidadeEmOrdem from tbProdutos where id=@idProduto)
			set @Desconto=(select Desconto from tbProdutos where id=@idProduto)

			insert into tbPedidosxProdutos (idProduto,idPedido,Quantidade,Preco,Desconto) Values
			(@idProduto,@idPedido,@Quantidade,@Preco,@Desconto);

			update tbProdutos set
			Quantidade=@Estoque-@Quantidade,
			QuantidadeEmOrdem=@QuantidadeEmOrdem+@Quantidade
			where id =@idProduto
		end

end
GO
--Confere os itens em estoque e se poss�vel insere junto com o pre�o e desconto do momento
create trigger trg_Update_ItenPedido on tbPedidosxProdutos instead of update as
begin
	declare @idPedido int
	declare @idProduto int 
	declare @Estoque int
	declare @Quantidade int
	declare @QuantidadeEmOrdem int
	declare @QtPedidoAnterior int
	declare @Preco money
	declare @Desconto real

	set @idPedido = (select idPedido from inserted);
	set @idProduto = (select idProduto from inserted);
	set @Quantidade = (select Quantidade from inserted)
	set @QtPedidoAnterior=(select Quantidade from tbPedidosxProdutos where idProduto=@idProduto and idPedido=@idPedido)
	set @Estoque=(select Quantidade from tbProdutos where id=@idProduto)
	set @Estoque=@Estoque+@QtPedidoAnterior
	if @Estoque>=@Quantidade
		begin
			set @Preco=(select Preco from tbProdutos where id=@idProduto)
			set @QuantidadeEmOrdem=(select QuantidadeEmOrdem from tbProdutos where id=@idProduto)
			set @QuantidadeEmOrdem=@QuantidadeEmOrdem-@QtPedidoAnterior
			set @Desconto=(select Desconto from tbProdutos where id=@idProduto)

			update tbPedidosxProdutos set
			Preco=@Preco,
			Desconto=@Desconto,
			Quantidade=@Quantidade
			where idProduto=@idProduto and idPedido=@idPedido

			update tbProdutos set
			Quantidade=@Estoque-@Quantidade,
			QuantidadeEmOrdem=@QuantidadeEmOrdem+@Quantidade
			where id =@idProduto
		end

end
GO
create trigger trg_Delete_Produto on tbProdutos instead of delete
as
begin
	print 'Entrou na trigger'
	declare @idProduto int;
	declare @idPedido int;

	set @idProduto = (select id from deleted);

	declare cursorItemPedido cursor for select idProduto from tbPedidosxProdutos where idProduto=@idProduto;


	open cursorItemPedido;

	fetch next from cursorItemPedido into @idPedido;

		while @@FETCH_STATUS = 0  
			begin
				delete from tbPedidosxProdutos where idProduto=@idProduto
				
				fetch next from cursorItemPedido into @idPedido;
			end
			close cursorItemPedido;
			deallocate cursorItemPedido;


	/*
		executa os deletes
	*/
	delete from tbProdutos where id=@idProduto
	
end
Go