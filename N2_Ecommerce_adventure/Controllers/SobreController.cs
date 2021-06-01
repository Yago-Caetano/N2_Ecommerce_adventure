using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Controllers
{
    public class SobreController : Controller
    {
        public IActionResult Index()
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

            return View();
        }
    }
}
