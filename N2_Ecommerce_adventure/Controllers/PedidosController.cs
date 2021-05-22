using Microsoft.AspNetCore.Mvc;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static N2_Ecommerce_adventure.DAO.PedidosDAO;

namespace N2_Ecommerce_adventure.Controllers
{
    public class PedidosController:PadraoController<PedidosViewModel>
    {
        public PedidosController()
        {
            DAO = new PedidosDAO();
            GeraProximoId = true;
        }
        public override IActionResult Index()
        {
            try
            {
                PedidosDAO dao = new PedidosDAO();
                var lista = dao.Listar(Model.Informacaoes);
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
}
