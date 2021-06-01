using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace N2_Ecommerce_adventure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        public  IActionResult Index(int? pagina=null)
        {
            const int ItensPorPagina = 25;
            try
            {
                ViewBag.CategoriasHeader = HelperControllers.CarregaCategoriasCabecalho();
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
                ViewBag.catProduto = -1;
                var lista = new ProdutosDAO().Listagem();
                int numeroPagina = (pagina ?? 1);
                return View("Index", lista.ToPagedList(numeroPagina, ItensPorPagina));
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        

        public IActionResult AplicaFiltro(int? pagina = null, bool newPage=false,int idCategoria=-1,String Nome="",String precoInicial=null, String precoFinal=null)
        {
            const int ItensPorPagina = 25;
            ViewBag.CategoriasHeader = HelperControllers.CarregaCategoriasCabecalho();

            ProdutosDAO mDAO = new ProdutosDAO();
            object precoIniAux, precoFinAux;

            if (precoInicial == null)
            {
                precoIniAux = DBNull.Value;
            }
            else
            {
                precoIniAux = precoInicial;
            }

            if (precoFinal == null)
            {
                precoFinAux = DBNull.Value;
            }
            else
            {
                precoFinAux = precoFinal;
            }

            //parametros
            SqlParameter[] mParams = new SqlParameter[]{
                                        new SqlParameter("Nome",(Nome == null ? "" : Nome)),
                                        new SqlParameter("idCategoria", idCategoria),
                                        new SqlParameter("ordem","Nome asc"),
                                        new SqlParameter("PrecoInicial", precoIniAux),
                                        new SqlParameter("PrecoFinal", precoFinAux),
                                    };

            var lista = mDAO.Filtro(mParams);
            int numeroPagina = (pagina ?? 1);

            if (!newPage)
            {
                return PartialView("pvHomeProdutos", lista.ToPagedList(numeroPagina, ItensPorPagina));
            }
            else
            {
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

                List<Object> filtros = new List<object>();
                filtros.Add(newPage);
                filtros.Add(idCategoria);
                filtros.Add(Nome);
                filtros.Add(precoInicial);
                filtros.Add(precoFinal);
                ViewBag.Filtros = filtros;
                ViewBag.catProduto = idCategoria;
                if(lista != null)
                {
                    return View("Index", lista.ToPagedList(numeroPagina, ItensPorPagina));
                }
                else
                {
                    return View("Index", lista);
                }
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
