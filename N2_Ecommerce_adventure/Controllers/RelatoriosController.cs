using Microsoft.AspNetCore.Http;
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
using X.PagedList;

namespace N2_Ecommerce_adventure.Controllers
{
    public class RelatoriosController : Controller
    {
        public virtual IActionResult Index()
        {
            try
            {
                //verifica se o usuario está logado e o nivel de acesso
                if (HelperControllers.VerificaUserLogado(HttpContext.Session))
                {
                    ViewBag.Logado = true;
                    ViewBag.Tipo = HttpContext.Session.GetString("Tipo");
                }
                else
                {
                    ViewBag.Logado = null;
                    ViewBag.Tipo = "Normal";
                }

                return View("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.CategoriasHeader = HelperControllers.CarregaCategoriasCabecalho();
            if (!HelperControllers.VerificaUserLogado(HttpContext.Session))
                context.Result = RedirectToAction("Index", "Login");
        }



    public IActionResult ConsultaAjax(string tipo,DateTime dataInicial,DateTime dataFinal,string nome, int? pagina = null)
        {
            const int ItensPorPagina = 2;
            try
            {
                    int numeroPagina = (pagina ?? 1);


                    if (tipo == "Pedidos em Aberto")
                    {
                        PedidosDAO mPedidos = new PedidosDAO();
                        var lista = mPedidos.GetAll(1);
                        ViewBag.Somatoria = mPedidos.GetSomatoriaValor(1);
                        return PartialView("pvConteudoPedidos", lista.ToPagedList(numeroPagina, ItensPorPagina));

                }
                else if(tipo == "Pedidos Concluido")
                    {
                        PedidosDAO mPedidos = new PedidosDAO();
                        var lista = mPedidos.GetAll(2);
                        ViewBag.Somatoria = mPedidos.GetSomatoriaValor(2);
                        return PartialView("pvConteudoPedidos", lista.ToPagedList(numeroPagina, ItensPorPagina));

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
                        return PartialView("pvConteudoProdutos", lista.ToPagedList(numeroPagina, ItensPorPagina));

                }
                else
                    {
                        ProdutosViewModel pvModel = new ProdutosViewModel();
                        EstoqueDAO mEstoques = new EstoqueDAO();
                        ParametroFiltro param = new ParametroFiltro();
                        var lista = mEstoques.ListagemView("view_Estoque"); // retorna todos os registro
                        return PartialView("pvEstoque", lista.ToPagedList(numeroPagina, ItensPorPagina));

                }


            }
            catch (Exception e)
            {
                return Json(new { erro = true });
            }
        }
    }
}
