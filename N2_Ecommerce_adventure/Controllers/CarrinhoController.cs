using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Controllers
{
    public class CarrinhoController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                ViewBag.CategoriasHeader = HelperControllers.CarregaCategoriasCabecalho();

                List<CarrinhoViewModel> carrinho = ObtemCarrinhoNaSession();
                //@ViewBag.TotalCarrinho = carrinho.Sum(c => c.Quantidade);
                @ViewBag.TotalCarrinho = 0;
                foreach (var c in carrinho)
                    @ViewBag.TotalCarrinho += c.Quantidade;

                return View("Index", carrinho);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public IActionResult Detalhes(int idProduto)
        {
            try
            {
                ViewBag.CategoriasHeader = HelperControllers.CarregaCategoriasCabecalho();

                List<CarrinhoViewModel> carrinho = ObtemCarrinhoNaSession();
                ProdutosDAO prodDAO = new ProdutosDAO();
                var modelProduto = prodDAO.Consulta(idProduto);
                CarrinhoViewModel carrinhoModel = carrinho.Find(c => c.ProdutoId == idProduto);
                if (carrinhoModel == null)
                {
                    carrinhoModel = new CarrinhoViewModel();
                    carrinhoModel.ProdutoId = idProduto;
                    carrinhoModel.Nome = modelProduto.Nome;
                    carrinhoModel.Quantidade = 0;
                }
                // preenche a imagem
                carrinhoModel.ImagemEmBase64 = modelProduto.FotoEmBase64;
                return View(carrinhoModel);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        private List<CarrinhoViewModel> ObtemCarrinhoNaSession()
        {
            List<CarrinhoViewModel> carrinho = new List<CarrinhoViewModel>();
            string carrinhoJson = HttpContext.Session.GetString("Carrinho");
            if (carrinhoJson != null)
                carrinho = JsonConvert.DeserializeObject<List<CarrinhoViewModel>>(carrinhoJson);
            return carrinho;
        }
        public IActionResult AdicionarCarrinho(int ProdutoId, int Quantidade)
        {
            try
            {
                ViewBag.CategoriasHeader = HelperControllers.CarregaCategoriasCabecalho();

                List<CarrinhoViewModel> carrinho = ObtemCarrinhoNaSession();
                CarrinhoViewModel carrinhoModel = carrinho.Find(c => c.ProdutoId == ProdutoId);
                if (carrinhoModel != null && Quantidade == 0)
                {
                    //tira do carrinho
                    carrinho.Remove(carrinhoModel);
                }
                else if (carrinhoModel == null && Quantidade > 0)
                {
                    //não havia no carrinho, vamos adicionar
                    ProdutosDAO pDAO = new ProdutosDAO();
                    var model = pDAO.Consulta(ProdutoId);
                    carrinhoModel = new CarrinhoViewModel();
                    carrinhoModel.ProdutoId = ProdutoId;
                    carrinhoModel.Nome = model.Nome;
                    carrinhoModel.Quantidade = Quantidade;
                    carrinho.Add(carrinhoModel);
                }
                if (carrinhoModel != null)
                    carrinhoModel.Quantidade = Quantidade;
                string carrinhoJson = JsonConvert.SerializeObject(carrinho);
                HttpContext.Session.SetString("Carrinho", carrinhoJson);
                return RedirectToAction("Visualizar");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private void PreparaListaEnderecosParaCombo(int idUsuario)
        {

            var enderecos = new UsuarioDAO().GetUsuariosEnderecos(idUsuario);

            List<SelectListItem> listaEnderecos = new List<SelectListItem>();
            listaEnderecos.Add(new SelectListItem("Selecione um Endereço", "-1"));
            foreach (var endereco in enderecos)
            {
                SelectListItem item = new SelectListItem(endereco.Rua + " " + endereco.Complemento, endereco.Id.ToString());
                listaEnderecos.Add(item);
            }
            ViewBag.Enderecos = listaEnderecos;
        }

        public IActionResult Visualizar()
        {
            try
            {
                ViewBag.CategoriasHeader = HelperControllers.CarregaCategoriasCabecalho();

                ProdutosDAO dao = new ProdutosDAO();
                var carrinho = ObtemCarrinhoNaSession();
                foreach (var item in carrinho)
                {
                    var cid = dao.Consulta(item.ProdutoId);
                    item.ImagemEmBase64 = cid.FotoEmBase64;
                }


                PreparaListaEnderecosParaCombo(HelperControllers.GetUserLogadoID(HttpContext.Session));

                return View(carrinho);
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
            else
            {
                ViewBag.Logado = true;
                ViewBag.CategoriasHeader = HelperControllers.CarregaCategoriasCabecalho();

                base.OnActionExecuting(context);
            }
        }


        public IActionResult EfetuarPedido(int idEndereco)
        {
            try
            {
                using (var transacao = new System.Transactions.TransactionScope())
                {
                    PedidosViewModel pedido = new PedidosViewModel();
                    pedido.data = DateTime.Now;
                    PedidosDAO pedidoDAO = new PedidosDAO();

                    //status do pedido
                    StatusPedidoViewModel mStatus = new StatusPedidoViewModel();
                    mStatus.Id = 1;

                    //Endereço 
                    EnderecoViewModel endereco = new EnderecoViewModel();
                    endereco.Id = idEndereco;

                    //Usuario
                    UsuarioSimplificadoViewModel user = new UsuarioSimplificadoViewModel();
                    user.Id = HelperControllers.GetUserLogadoID(HttpContext.Session);

                    pedido.status = mStatus;
                    pedido.endereco = endereco;

                    pedido.Cliente = user;
                    int idNovoPedido = pedidoDAO.Insert(pedido,true);
           
                    ProdutoPedidoDAO itemDAO = new ProdutoPedidoDAO();
                    var carrinho = ObtemCarrinhoNaSession();
                    foreach (var elemento in carrinho)
                    {
                        ProdutoPedidoViewModel item = new ProdutoPedidoViewModel();
                        item.idPedido = idNovoPedido;
                        item.Produto.Id = elemento.ProdutoId;
                        item.Quantidade = elemento.Quantidade;
                        itemDAO.Insert(item);
                    }
                    transacao.Complete();
                }

                HelperControllers.LimparCarrinho(HttpContext.Session);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

    }
}