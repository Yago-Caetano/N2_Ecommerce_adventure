using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Models
{
    public class EstoqueModel:PadraoViewModel
    {
        public String Nome { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadeEmOrdem { get; set; }


    }
}
