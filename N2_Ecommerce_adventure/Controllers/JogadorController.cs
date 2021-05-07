using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace N2_Ecommerce_adventure.Controllers
{
    public class JogadorController : PadraoController<JogadorViewModel>
    {
        public JogadorController()
        {
            DAO = new JogadorDAO();
        }

        protected override void PreencheDadosParaView(string Operacao, JogadorViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            PreparaListaPosicoesParaCombo();
        }
        protected override void ValidaDados(JogadorViewModel model, string operacao)
        {
            PreparaListaPosicoesParaCombo();

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Preencha o Nome");
            if (model.Posicao <= 0)
                ModelState.AddModelError("Posicao", "Campo obrigatório!");
            if (model.TimeId <= 0)
                ModelState.AddModelError("TimeId", "Campo obrigatório!");
        }

        private void PreparaListaPosicoesParaCombo()
        {
            TimeFutebolDAO timefutebolDAO = new TimeFutebolDAO();
            var times = timefutebolDAO.Listagem();
            List<SelectListItem> listaTimesFutebol = new List<SelectListItem>();
            listaTimesFutebol.Add(new SelectListItem("Selecione uma categoria...", "0"));
            foreach (var time in times)
            {
                SelectListItem item = new SelectListItem(time.Nome, time.Id.ToString());
                listaTimesFutebol.Add(item);
            }
            ViewBag.TimesFutebol = listaTimesFutebol;
        }

    }
}
