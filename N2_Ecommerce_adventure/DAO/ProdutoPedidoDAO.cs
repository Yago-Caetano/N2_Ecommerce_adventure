using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class ProdutoPedidoDAO : PadraoDAO<ProdutoPedidoViewModel>
    {
        protected override SqlParameter[] CriaParametros(ProdutoPedidoViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("idPedido", model.idPedido);
            parametros[1] = new SqlParameter("idProduto ", model.Produto.Id);
            parametros[2] = new SqlParameter("Quantidade ", model.Quantidade);
            return parametros;
        }

        protected override ProdutoPedidoViewModel MontaModel(DataRow registro)
        {
            ProdutoPedidoViewModel produtoPedido = new ProdutoPedidoViewModel();
            produtoPedido.idPedido = Convert.ToInt32(registro["idPedido"]);
            produtoPedido.Produto.Id = Convert.ToInt32(registro["idProduto"]);
            produtoPedido.Quantidade = Convert.ToInt32(registro["Quantidade"]);
            produtoPedido.Desconto = Convert.ToDouble(registro["Desconto"]);
            produtoPedido.Preco = Convert.ToDouble(registro["Preco"]);

            if (produtoPedido.Produto.Id != 0)
                produtoPedido.Produto = GetProdutoSimplificado(produtoPedido.Produto.Id);

            return produtoPedido;
        }
        public virtual List<ProdutoPedidoViewModel> Listagem(int idPedido)
        {
            List<ProdutoPedidoViewModel> lista = new List<ProdutoPedidoViewModel>();
            var p = new SqlParameter[]
            {
                new SqlParameter("idPedido", idPedido),
            };
            var tabela = HelperDAO.ExecutaProcSelect("splistar_itensPedido", p);
            if (tabela.Rows.Count == 0)
                return lista;
            else
            {
                foreach (DataRow registro in tabela.Rows)
                {
                    lista.Add(MontaModel(registro));
                }
            }

            return lista;
        }
        public ProdutoPedidoViewModel Consulta(int id, int idProduto)
        {
            ProdutoPedidoViewModel pedido = new ProdutoPedidoViewModel();
            var p = new SqlParameter[]
            {
                new SqlParameter("idPedido", id),
                new SqlParameter("idProduto", idProduto)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_tbPedidosxProdutos", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
            return pedido;
        }
        private ProdutoSimplificadoViewModel GetProdutoSimplificado(int id)
        {
            ProdutoSimplificadoViewModel produto = new ProdutoSimplificadoViewModel();
            ProdutosDAO tipoDAO = new ProdutosDAO();
            produto = tipoDAO.ConsultaNome(id);
            return produto;
        }

        protected override void SetTabela()
        {
            Tabela = "tbPedidosxProdutos";
        }
    }
}
