using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Controllers
{
    public class UsuarioController:PadraoController<UsuarioViewModel>
    {
        public UsuarioController()
        {
            DAO = new UsuarioDAO();
            GeraProximoId = true;
            ExibeAutenticacao = false;
        }
        private void PreparaListaPosicoesParaCombo()
        {
            TipoUsuarioDAO tipos = new TipoUsuarioDAO();
            var categorias = tipos.Listagem();
            List<SelectListItem> listaCategorias = new List<SelectListItem>();
            foreach (var categoria in categorias)
            {
                SelectListItem item = new SelectListItem(categoria.Tipo, categoria.Id.ToString());
                listaCategorias.Add(item);
            }
            ViewBag.Categorias = listaCategorias;
        }
        protected override void PreencheDadosParaView(string Operacao, UsuarioViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            PreparaListaPosicoesParaCombo();
        }

        protected override void ValidaDados(UsuarioViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if(model.Nome == null)
                ModelState.AddModelError("Nome", "Preencha o Nome!");

            if(model.Nascimento == Convert.ToDateTime("01/01/0001"))
                ModelState.AddModelError("Nascimento", "Preencha a Data!");

            if (model.Email == null)
                ModelState.AddModelError("Email", "Preencha o Email!");

            if (model.senha == null)
                ModelState.AddModelError("senha","Preencha a senha!");
            if (model.senhaRepetida == null)
                ModelState.AddModelError("senhaRepetida", "Preencha a senha!");

            if (model.CPF == null)
                ModelState.AddModelError("CPF", "Preencha o CPF!");
        }

        public IActionResult CarregarPerfil()
        {
            UsuarioViewModel user = new UsuarioViewModel();
            user = DAO.Consulta(Convert.ToInt32(HttpContext.Session.GetString("Logado")));
            PreparaListaEnderecosParaCombo(user);
            return View("Perfil", user);
        }

        private void PreparaListaEnderecosParaCombo(UsuarioViewModel user)
        {
            var enderecos = user.Enderecos;
            List<SelectListItem> listaEnderecos = new List<SelectListItem>();
            listaEnderecos.Add(new SelectListItem("Selecione um Endereço", "0"));
            foreach (var endereco in enderecos)
            {
                SelectListItem item = new SelectListItem(endereco.Rua + " " + endereco.Complemento, endereco.Id.ToString());
                listaEnderecos.Add(item);
            }
            ViewBag.Enderecos = listaEnderecos;
        }

        public IActionResult FazConsultaEnderecoAjax(int idEndereco)
        {
            try
            {
                EnderecoDAO eDAO = new EnderecoDAO();
                EnderecoViewModel endereco = new EnderecoViewModel();
                 endereco = eDAO.Consulta(idEndereco); // retorna todos os registro
                ViewBag.EnderecoDados = endereco;
                return PartialView("pvEndereco", endereco);
            }
            catch
            {
                return Json(new { erro = true });
            }
        }

    }
}
