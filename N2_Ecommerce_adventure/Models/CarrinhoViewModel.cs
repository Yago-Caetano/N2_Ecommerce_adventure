using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Models
{
    public class CarrinhoViewModel
    {
        public int Quantidade { get; set; }
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string ImagemEmBase64 { get; set; }
    }
}
