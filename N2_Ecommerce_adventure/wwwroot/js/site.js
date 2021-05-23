
function apagarRegistro(id,controller) {

    swal({
        title: "Tem certeza?",
        text: "O registro será apagado para sempre!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        cancelButtonClass: "btn-info",
        confirmButtonText: "Sim",        
        cancelButtonText: "Não!",
        closeOnConfirm: false
    },
        function () {
            location.href = '/' + controller+'/Delete?id=' + id;
        });
}
function apagarItemPedido(id,idProduto,produto) {

    swal({
        title: "Tem certeza?",
        text: "O item: "+ produto+ "será retirado do pedido",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        cancelButtonClass: "btn-info",
        confirmButtonText: "Sim",
        cancelButtonText: "Não!",
        closeOnConfirm: false
    },
        function () {
            location.href = '/' + controller + '/DeleteItens?id=' + id + '&idProduto=' + idProduto;
        });
}