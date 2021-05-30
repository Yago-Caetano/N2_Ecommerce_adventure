using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace N2_Ecommerce_adventure.Controllers
{
    public class PadraoController<T> : Controller where T : PadraoViewModel
    {
        protected PadraoDAO<T> DAO { get; set; }
        protected bool GeraProximoId { get; set; }
        protected string NomeViewIndex { get; set; } = "index";

        protected string NomeViewForm { get; set; } = "form";

        protected bool ExibeAutenticacao { get; set; } = true;


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

                var lista = DAO.Listagem();
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public virtual IActionResult Create()
        {
            try
            {
                ViewBag.Operacao = "I";
                T model = Activator.CreateInstance(typeof(T)) as T;
                PreencheDadosParaView("I", model);
                return View(NomeViewForm, model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        protected virtual void PreencheDadosParaView(string Operacao, T model)
        {
            if (GeraProximoId && Operacao == "I")
                model.Id = DAO.ProximoId();
        }
        public virtual IActionResult Save(T model, string Operacao)
        {
            try
            {
                ValidaDados(model, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreencheDadosParaView(Operacao, model);
                    return View(NomeViewForm, model);
                }
                else
                {
                    if (Operacao == "I")
                        DAO.Insert(model);
                    else
                        DAO.Update(model);
                    return RedirectToAction(NomeViewIndex);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected virtual void ValidaDados(T model, string operacao)
        {
            ModelState.Clear();

            if (operacao == "I" && DAO.Consulta(model.Id) != null)
                ModelState.AddModelError("Id", "Código já está em uso!");
            if (operacao == "A" && DAO.Consulta(model.Id) == null)
                ModelState.AddModelError("Id", "Este registro não existe!");
            if (model.Id <= 0)
                ModelState.AddModelError("Id", "Id inválido!");
        }

        public IActionResult Edit(int id)
        {
            try
            {
                ViewBag.Operacao = "A";
                var model = DAO.Consulta(id);
                if (model == null)
                    return RedirectToAction(NomeViewIndex);
                else
                {
                    PreencheDadosParaView("A", model);
                    return View(NomeViewForm, model);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public IActionResult Delete(int id)
        {
            try
            {
                DAO.Delete(id);
                return RedirectToAction(NomeViewIndex);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.Tipo = HttpContext.Session.GetString("Tipo");
            ViewBag.Logado = HttpContext.Session.GetString("Logado");

            if(context.RouteData.Values["action"].ToString() == "Detalhes" && context.RouteData.Values["controller"].ToString() == "Produtos")
            {
                base.OnActionExecuting(context);
                return;
            }

            if (!HelperControllers.VerificaUserLogado(HttpContext.Session) && context.RouteData.Values["action"].ToString() == "Index" && context.RouteData.Values["controller"].ToString() == "Usuario")
            {
                context.Result = RedirectToAction("Index", "Login");
            }


            if (ExibeAutenticacao && !HelperControllers.VerificaUserLogado(HttpContext.Session))
                context.Result = RedirectToAction("Index", "Login");
            else
            {
                ViewBag.Logado = true;
                base.OnActionExecuting(context);
            }
        }
    }
}
