using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Controllers
{
    public class ProdutosController:PadraoController<ProdutosViewModel>
    {
        public ProdutosController()
        {
            DAO = new ProdutosDAO();
            GeraProximoId = true;
        }
        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

        public IActionResult Detalhes(int id)
        {
            try
            {
                var model = DAO.Consulta(id);

                return View("Detalhes", model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }

        }
        public override IActionResult Save(ProdutosViewModel model, string Operacao)
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
                    {
                        if (model.Foto != null)
                            model.FotoEmByte = ConvertImageToByte(model.Foto);
                        DAO.Insert(model);
                    }

                    else
                    {
                        ProdutosViewModel p = new ProdutosViewModel();
                        p = DAO.Consulta(model.Id);

                        if (model.Foto == null && p.FotoEmByte != null)
                            model.FotoEmByte = p.FotoEmByte;

                        DAO.Update(model);
                    }
                        
                    return RedirectToAction(NomeViewIndex);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        private void PreparaListaCategoriaParaCombo()
        {
            CategoriaProdutoDAO tipos = new CategoriaProdutoDAO();
            var categorias = tipos.Listagem();
            List<SelectListItem> listaCategorias = new List<SelectListItem>();
            foreach (var categoria in categorias)
            {
                SelectListItem item = new SelectListItem(categoria.Categoria, categoria.Id.ToString());
                listaCategorias.Add(item);
            }
            ViewBag.Categorias = listaCategorias;
        }
        protected override void PreencheDadosParaView(string Operacao, ProdutosViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            PreparaListaCategoriaParaCombo();
        }

        protected override void ValidaDados(ProdutosViewModel produtos, string operacao)
        {
            base.ValidaDados(produtos, operacao);   

            if (string.IsNullOrEmpty(produtos.Nome))
                ModelState.AddModelError("Nome", "Preencha o nome do produto.");
            if (produtos.FotoEmBase64 == null)
                ModelState.AddModelError("Foto", "Campo obrigatório.");
            if (produtos.Preço <= 0)
                ModelState.AddModelError("Preço", "Informe o preço.");
            if (produtos.Quantidade <= 0)
                ModelState.AddModelError("Quantidade", "Quantidade inválida");
            if (produtos.Desconto < 0)
                ModelState.AddModelError("Desconto", "Desconto inválido.");
            if (string.IsNullOrEmpty(produtos.Descricao))
                ModelState.AddModelError("Descricao", "Preencha a descrição.");
            if (produtos.Categoria_Produto == null)
                ModelState.AddModelError("Categoria_Produto", "Selecione uma categoria.");
        }

    }
}
