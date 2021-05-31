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
    public class EnderecoController : PadraoController<EnderecoViewModel>
    {
        public EnderecoController()
        {
            DAO = new EnderecoDAO();
            GeraProximoId = true;
        }
        public override IActionResult Index()
        {
            try
            {
                SetUserData();
                var lista = DAO.Listagem();
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public override IActionResult Save(EnderecoViewModel model, string Operacao)
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
                        (DAO as EnderecoDAO).InsertEndereco(model, HelperControllers.GetUserLogadoID(HttpContext.Session));
                    else
                        DAO.Update(model);

                    if (HttpContext.Session.GetString("Tipo") == "Gerente")
                        return RedirectToAction(NomeViewIndex);
                    else
                        return RedirectToAction("CarregarPerfil", "Usuario");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected override void PreencheDadosParaView(string Operacao, EnderecoViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            SetUserData();
        }
        private void SetUserData()
        {
            UsuarioDAO userdao = new UsuarioDAO();
            ViewBag.IDusuario = HelperControllers.GetUserLogadoID(HttpContext.Session);
            ViewBag.NomeUsuario = userdao.GetSimplifiedUser(ViewBag.IDusuario).Nome;
        }

        protected override void ValidaDados(EnderecoViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (model.Rua == null)
                ModelState.AddModelError("Rua", "Campo Obrigatório!");

            if (model.Complemento == null)
                ModelState.AddModelError("Complemento", "Campo Obrigatório!");

            if (model.Numero == 0)
                ModelState.AddModelError("Numero", "Insira um Número maior que Zero!");

            if (model.CEP == null)
                ModelState.AddModelError("CEP", "Campo Obrigatório!");

            try
            {
                string valor = model.CEP.Replace(".", "");
                valor = valor.Replace("-", "");

                if (!Int32.TryParse(valor, out int j))
                    ModelState.AddModelError("CEP", "CEP Inválido!");
                
                
            }
            catch (Exception e)
            {
                ModelState.AddModelError("CEP", "Campo Obrigatório!");
            }

            if (model.Cidade == null)
                ModelState.AddModelError("Cidade", "Campo Obrigatório!");

        }

    }

}
