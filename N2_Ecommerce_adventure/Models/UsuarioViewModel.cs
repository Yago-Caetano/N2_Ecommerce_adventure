using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Models
{
    public class UsuarioViewModel : PadraoViewModel
    {
        public UsuarioViewModel()
        {
            Enderecos = new List<EnderecoViewModel>();
        }
        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
        public string Email { get; set; }
        public string senha { get; set; }
        //só utilizada para  criação de usuario
        public string senhaRepetida { get; set; }
        public string CPF { get; set; }
        public int idTipoUsuario { get; set; }
        public bool Ativado { get; set; } = true;
        public TipoUsuarioViewModel Tipo_Usuario {get;set;}
        public List<EnderecoViewModel> Enderecos { get; set; }


        

    }
}
