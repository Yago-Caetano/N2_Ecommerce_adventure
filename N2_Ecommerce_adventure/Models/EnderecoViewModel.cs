using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Models
{
    public class EnderecoViewModel :PadraoViewModel
    {
        public string Rua { get; set; }
        public string Complemento { get; set; }
        public int Numero { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public bool Ativo { get; set; } = true;
        public string NomeUsuario { get; set; }
        public int IDUsuario_logado { get; set; }
    }
}
