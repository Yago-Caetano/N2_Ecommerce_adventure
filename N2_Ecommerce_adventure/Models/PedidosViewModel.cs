using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Models
{
    public class PedidosViewModel : PadraoViewModel
    {
        public PedidosViewModel()
        {
            status = new StatusPedidoViewModel();
            Cliente = new UsuarioSimplificadoViewModel();
            endereco = new EnderecoViewModel();
            Itens = new List<ProdutoPedidoViewModel>();
        }
        public StatusPedidoViewModel status { get; set; }
        public UsuarioSimplificadoViewModel Cliente { get; set; }

        public EnderecoViewModel endereco { get; set; }

        public List<ProdutoPedidoViewModel> Itens { get; set; }

        public DateTime data { get; set; }

        public Double Valor { get; set; }



    }
}
