using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Models
{
    //Usuario só com informações de cadastro
    public class UsuarioSimplificadoViewModel:PadraoViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }

    }
}
