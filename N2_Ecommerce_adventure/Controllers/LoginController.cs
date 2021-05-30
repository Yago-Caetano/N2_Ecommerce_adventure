using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Tipo = null;
            return View();
        }
        public IActionResult FazLogin(string usuario, string senha)
        {
            UsuarioDAO dao = new UsuarioDAO();
            UsuarioViewModel user = new UsuarioViewModel();
            user = dao.VerificaUsuario(usuario, senha);
            //Este é apenas um exemplo, aqui você deve consultar na sua tabela de usuários
            //se existe esse usuário e senha
            if (user != null)
            {
                HttpContext.Session.SetString("Logado", user.Id.ToString());
                HttpContext.Session.SetString("Tipo", user.Tipo_Usuario.Tipo);
                ViewBag.Tipo = HttpContext.Session.GetString("Tipo");
                return RedirectToAction("index","Home");
            }
            else
            {
                ViewBag.Erro = "Usuário ou senha inválidos!";
                return View("Index");
            }
        }
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
