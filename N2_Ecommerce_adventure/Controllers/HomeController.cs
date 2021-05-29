﻿using Microsoft.AspNetCore.Mvc;
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

namespace N2_Ecommerce_adventure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public  IActionResult Index()
        {
            try
            {
                ViewBag.catProduto = -1;
                var lista = new ProdutosDAO().Listagem();
                return View("Index", lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult AplicaFiltro(int idCategoria=-1,String Nome="",String precoInicial=null, String precoFinal=null)
        {
            
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
            return PartialView("pvHomeProdutos", lista);
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
