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
    }
}
