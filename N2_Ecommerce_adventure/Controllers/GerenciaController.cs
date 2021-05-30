using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Controllers
{
    public class GerenciaController:Controller
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
            if (!HelperControllers.VerificaUserLogado(HttpContext.Session))
                context.Result = RedirectToAction("Index", "Login");
        }
    }
}
