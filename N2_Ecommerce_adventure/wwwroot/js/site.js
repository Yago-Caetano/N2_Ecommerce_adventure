

function efetuaFiltro() {
	var selectTipo = $("#select-tipo").val();
	var dataInicial = $("#input-data-ini").val();
	var dataFinal = $("#input-data-fim").val();



	var url = `/Relatorios/ConsultaAjax?tipo=${selectTipo}` + (dataInicial.lenght > 1 ? `&dataInicial=${dataInicial}` : '') + (dataFinal.lenght > 1 ? `&dataFinal=${dataFinal}` : '');
	
	$.ajax({
		url: url,
		cache: false,
		beforeSend: function () {
			$("#imgWait").show();
		},
		success: function (dados) {
			//$("#imgWait").hide();
			if (dados.erro != undefined) 
			{
				alert('Ocorreu um erro ao processar a sua requisição. Tente novamente mais tarde..');
			}
			else 		   
			{
				$("#conteudoGrid").html(dados);
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

