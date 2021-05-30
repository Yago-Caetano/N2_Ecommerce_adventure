

function efetuaFiltro() {
	var selectTipo = $("#select-tipo").val();
	var dataInicial = $("#input-data-ini").val();
	var dataFinal = $("#input-data-fim").val();
	var nome = $("#input-txt-nome").val();


	var url = `/Relatorios/ConsultaAjax?tipo=${selectTipo}` + (dataInicial.lenght > 1 ? `&dataInicial=${dataInicial}` : '') + (dataFinal.lenght > 1 ? `&dataFinal=${dataFinal}` : '')
				+ (nome.length > 1 ? `&nome=${nome}`: '');
	
	$.ajax({
		url: url,
		cache: false,
		beforeSend: function () {
			
		},
		success: function (dados) {
			//$("#imgWait").hide();
			if (dados.erro != undefined) 
			{
				alert('Ocorreu um erro ao processar a sua requisição. Tente novamente mais tarde..');
			}
			else 		   
			{
				$("#conteudo").html(dados);
			}
		}
	});
}


function efetuaFiltroUser() {
	var dataInicial = $("#input-data-ini").val();
	var dataFinal = $("#input-data-fim").val();
	var nome = $("#input-txt-nome").val();

	var url = `/Usuario/ConsultaFiltroAjax?dataInicial=${dataInicial}&dataFinal=${dataFinal}&Nome=${nome}` 

	$.ajax({
		url: url,
		cache: false,
		beforeSend: function () {

		},
		success: function (dados) {
			//$("#imgWait").hide();
			if (dados.erro != undefined) {
				alert('Ocorreu um erro ao processar a sua requisição. Tente novamente mais tarde..');
			}
			else {
				$("#conteudo").html(dados);
			}
		}
	});

}

function efetuaFiltroProdutos(idCategoria) {

	var valorInicial = $("#input-val-ini").val();
	var valorFinal = $("#input-val-fim").val();
	var nome = $("#input-txt-nome").val();


	var url = `/Home/AplicaFiltro?idCategoria=${idCategoria}&Nome=${nome}` + (valorInicial ? `&precoInicial=${valorInicial}` : '') + (valorFinal ? `&precoFinal=${valorFinal}` : '')

	$.ajax({
		url: url,
		cache: false,
		beforeSend: function () {

		},
		success: function (dados) {
			//$("#imgWait").hide();
			if (dados.erro != undefined) {
				alert('Ocorreu um erro ao processar a sua requisição. Tente novamente mais tarde..');
			}
			else {
				$("#conteudo").html(dados);
			}
		}
	});

}

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


function preencherEndereco() {
	var idEndereco = $("#idEndereco").val();

	$.ajax({
		url: "/Usuario/FazConsultaEnderecoAjax?idEndereco=" + idEndereco,
		cache: false,
		beforeSend: function () {
		},
		success: function (dados) {
			if (dados.erro != undefined)  
			{
				alert('Ocorreu um erro ao processar a sua requisição.');
			}
			else 		   
			{
				$("#conteudoEndereco").html(dados);
			}
		}
	});
}

function finalizaPed() {

	var endId = $("#idEndereco").val();
	//verifica se houve seleção de endereço válido
	if (endId == "-1") {
		swal({
			title: "Não há endereço selecionado",
			text: "Selecione um endereço para prosseguir!",
			type: "warning"

		});
	}
	else {
		window.location = `/Carrinho/EfetuarPedido?idEndereco=${endId}`;
    }
	//	/Carrinho/EfetuarPedido
}

