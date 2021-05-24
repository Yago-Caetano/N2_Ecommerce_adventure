using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
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

        public IActionResult ConsultaAjax(
                   string tipo,
                   DateTime dataInicial,
                   DateTime dataFinal)
        {


            try
            {
                switch(tipo)
                {
                    case "pedidos_abertos":

                        break;

                    case "pedidos_concluidos":

                        break;

                    case "estoque":

                        break;

                    case "produtos_cadastrados":

                        break;
                }
                Thread.Sleep(1000); // para dar tempo de ver o gif na tela..rs
                /*if (nomeAluno == null)
                    nomeAluno = "";
                var lista = (DAO as AlunoDAO).ListagemComFiltro(nomeAluno, cidade, dataInicial, dataFinal); // retorna todos os registro
                */
                return PartialView("pvConteudo", null);
            }
            catch
            {
                return Json(new { erro = true });
            }
        }
    }
}
