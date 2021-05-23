
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

