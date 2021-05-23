

//gerencia o click nos botões de navegação
function navigateTo(itemSelected){

    switch (itemSelected.id) {

        case "item-user":
            window.location.href = "/Usuario";
            break;

        case "item-prod":
            window.location.href = "/Produtos";
            break;

        case "item-ped":
            window.location.href = "/Pedidos";
            break;

        case "item-end":
            window.location.href = "/Endereco";
            break;
    }
};