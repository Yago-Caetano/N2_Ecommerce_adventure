using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Controllers
{
    public class TipoUsuarioController:PadraoController<TipoUsuarioViewModel>
    {
        public TipoUsuarioController()
        {
            DAO = new TipoUsuarioDAO();
            GeraProximoId = true;
        }
    }
}
