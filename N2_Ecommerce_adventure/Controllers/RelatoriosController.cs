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



    public IActionResult ConsultaAjax(string tipo,DateTime dataInicial,DateTime dataFinal,string nome)
        {

            try
            {
               
                    if(tipo == "Pedidos em Aberto")
                    {
                        PedidosDAO mPedidos = new PedidosDAO();
                        var lista = mPedidos.GetAll(1);
                        ViewBag.Somatoria = mPedidos.GetSomatoriaValor(1);
                        return PartialView("pvConteudoPedidos", lista);
                    }
                    else if(tipo == "Pedidos Concluido")
                    {
                        PedidosDAO mPedidos = new PedidosDAO();
                        var lista = mPedidos.GetAll(2);
                        ViewBag.Somatoria = mPedidos.GetSomatoriaValor(2);
                        return PartialView("pvConteudoPedidos", lista);
                    }
                    else if(tipo == "Produtos Cadastrados")
                    {
                        ProdutosDAO mProdutos = new ProdutosDAO();
                        SqlParameter[] param = new SqlParameter[4];
                        param[0] = new SqlParameter("Nome", (nome == null ? "" : nome));
                        param[1] = new SqlParameter("PrecoInicial", DBNull.Value);
                        param[2] = new SqlParameter("PrecoFinal", DBNull.Value);
                        param[3] = new SqlParameter("ordem", "Nome asc");
                        var lista = mProdutos.Filtro(param); // retorna todos os registro
                        ViewBag.TipoRelat = "produtos_cadastrados";
                        return PartialView("pvConteudoProdutos", lista);
                    }
                    else
                    {
                        ProdutosViewModel pvModel = new ProdutosViewModel();
                        EstoqueDAO mEstoques = new EstoqueDAO();
                        ParametroFiltro param = new ParametroFiltro();
                        var lista = mEstoques.ListagemView("view_Estoque"); // retorna todos os registro
                        return PartialView("pvEstoque", lista);
                }
               
            }
            catch(Exception e)
            {
                return Json(new { erro = true });
            }
        }
    }
}
