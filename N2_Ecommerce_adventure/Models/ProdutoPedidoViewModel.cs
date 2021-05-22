using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Models
{
    public class ProdutoPedidoViewModel:PadraoViewModel
    {
        public ProdutoPedidoViewModel()
        {
            Produto = new ProdutoSimplificadoViewModel();
        }
        public int idPedido { get; set; }



        public ProdutoSimplificadoViewModel Produto { get; set; }

        public int Quantidade { get; set; }
        public double Desconto { get; set; }
        public double Preco { get; set; }

        
    }
}
