using Microsoft.AspNetCore.Mvc;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
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
        public override IActionResult Index(int? pagina=null)
        {
            const int ItensPorPagina = 5;
            try
            {
                PedidosDAO dao = new PedidosDAO();
                var lista = dao.Listar(Model.Informacaoes);
                int numeroPagina = (pagina ?? 1);
                return View(NomeViewIndex, lista.ToPagedList(numeroPagina, ItensPorPagina));
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
       
        public IActionResult DeleteItens(int id, int idProduto)
        {
            try
            {
                ProdutoPedidoDAO dao = new ProdutoPedidoDAO();
                dao.Delete(id,idProduto);
                PedidosDAO dao2 = new PedidosDAO();
                var pedido = dao2.Consulta(id, Model.Completo);
                return View("ItensPedido", pedido);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public virtual IActionResult CreateItens(int id)
        {
            try
            {
                ViewBag.Operacao = "I";
                ProdutoPedidoDAO dao = new ProdutoPedidoDAO();
                ProdutoPedidoViewModel produto = new ProdutoPedidoViewModel();
                produto.idPedido = id;              
                return View("FormItem", produto);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public virtual IActionResult EditarItem(int id, int idProduto)
        {
            try
            {
                ViewBag.Operacao = "A";
                ProdutoPedidoDAO dao = new ProdutoPedidoDAO();
                ProdutoPedidoViewModel produto = new ProdutoPedidoViewModel();
                var model = dao.Consulta(id, idProduto);
                return View("FormItem", model);

            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
       
        public virtual IActionResult SaveItens(ProdutoPedidoViewModel model, string Operacao)
        {
            try
            {
                ProdutoPedidoDAO dao = new ProdutoPedidoDAO();
                //ValidaDados(model, Operacao);
                //if (ModelState.IsValid == false)
               // {
                  //  ViewBag.Operacao = Operacao;
                  //  PreencheDadosParaView(Operacao, model);
                  //  return View(NomeViewForm, model);
               // }
               // else
              //  {
                    if (Operacao == "I")
                        dao.Insert(model);
                    else
                        dao.Update(model);
                int id = model.idPedido;
                    return RedirectToAction("VerItens", new { id=model.idPedido});
               // }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

    }
}
