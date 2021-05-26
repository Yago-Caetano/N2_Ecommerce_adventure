using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Controllers
{
    public class RelatoriosController : Controller
    {
        public virtual IActionResult Index()
        {
            try
            {

                return View("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!HelperControllers.VerificaUserLogado(HttpContext.Session))
                context.Result = RedirectToAction("Index", "Login");
        }



    public IActionResult ConsultaAjax(string tipo,DateTime dataInicial,DateTime dataFinal)
        {

            try
            {
               
                    if(tipo == "Pedidos em Aberto")
                    {
                        PedidosDAO mPedidos = new PedidosDAO();
                        var lista = mPedidos.GetAll(1);
                        return PartialView("pvConteudo", lista);
                    }
                    else if(tipo == "Pedidos Concluido")
                    {
                        PedidosDAO mPedidos = new PedidosDAO();
                        var lista = mPedidos.GetAll(2);
                        return PartialView("pvConteudo", lista);
                    }
                    else if(tipo == "Produtos Cadastrados")
                    {
                        ProdutosViewModel pvModel = new ProdutosViewModel();
                        ProdutosDAO mProdutos = new ProdutosDAO();
                        ParametroFiltro param = new ParametroFiltro();
                        var lista = mProdutos.Filtro(pvModel, param); // retorna todos os registro
                        ViewBag.TipoRelat = "produtos_cadastrados";
                        return PartialView("pvConteudo", lista);
                    }
                    else
                    {
                        ProdutosViewModel pvModel = new ProdutosViewModel();
                        ProdutosDAO mProdutos = new ProdutosDAO();
                        ParametroFiltro param = new ParametroFiltro();
                        var lista = mProdutos.Filtro(pvModel, param); // retorna todos os registro
                        ViewBag.TipoRelat = "estoque";
                        return PartialView("pvConteudo", lista);
                    }
               
            }
            catch(Exception e)
            {
                return Json(new { erro = true });
            }
        }
    }
}
