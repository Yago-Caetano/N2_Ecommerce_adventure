using Microsoft.AspNetCore.Http;
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
    public class ListaPedidosClienteController : PadraoController<PedidosViewModel>
    {
        public ListaPedidosClienteController()
        {
            DAO = new PedidosDAO();
            GeraProximoId = false;       
        }
        public override IActionResult Index()
        {
            try
            {
                PedidosDAO dao = new PedidosDAO();

                UsuarioDAO daoUser = new UsuarioDAO();
                UsuarioViewModel user = new UsuarioViewModel();
                user = daoUser.Consulta(Convert.ToInt32(HttpContext.Session.GetString("Logado")));
                dao.UserId = user.Id;

                var lista = dao.ListarByCliente(Model.Informacaoes);
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public virtual IActionResult VerItens(int id)
        {
            try
            {
                PedidosDAO dao = new PedidosDAO();
                var pedido = dao.Consulta(id, Model.Completo);
                return View("ItensPedido", pedido);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
}
