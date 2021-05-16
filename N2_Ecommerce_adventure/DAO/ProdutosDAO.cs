using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class ProdutosDAO : PadraoDAO<ProdutosViewModel>
    {
        protected override SqlParameter[] CriaParametros(ProdutosViewModel model)
        {
            object imgByte = model.FotoEmByte;

            if (imgByte == null)
                imgByte = DBNull.Value;

            SqlParameter[] parametros = new SqlParameter[8];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("Nome", model.Nome);
            parametros[2] = new SqlParameter("Preco ", model.Preço);
            parametros[3] = new SqlParameter("Descricao ", model.Descricao);
            parametros[4] = new SqlParameter("Foto ", imgByte);
            parametros[5] = new SqlParameter("Quantidade ", model.Quantidade);
            parametros[6] = new SqlParameter("Desconto ", model.Desconto);
            parametros[7] = new SqlParameter("idCategoria ", model.idCategoria);
            return parametros;
        }

        protected override ProdutosViewModel MontaModel(DataRow registro)
        {
            ProdutosViewModel produto = new ProdutosViewModel();
            produto.Id = Convert.ToInt32(registro["id"]);
            produto.Nome = registro["Nome"].ToString();
            produto.Preço = Convert.ToDouble(registro["Preco"]);
            produto.Descricao = registro["Descricao"].ToString();
            produto.Quantidade = Convert.ToInt32(registro["Quantidade"]);
            produto.QuantidadeEmOrdem = Convert.ToInt32(registro["QuantidadeEmOrdem"]);
            produto.Desconto = Convert.ToDouble(registro["Desconto"]);
            produto.idCategoria = Convert.ToInt32(registro["idCategoria"]);

            if (registro["Foto"] != DBNull.Value)
                produto.FotoEmByte = registro["Foto"] as byte[];

            if (produto.idCategoria > 0)
                produto.Categoria_Produto = GetUserTipo(produto.idCategoria);

            return produto;
        }
        private CategoriaProdutoViewModel GetUserTipo(int id)
        {
            CategoriaProdutoViewModel categoria = new CategoriaProdutoViewModel();
            CategoriaProdutoDAO tipoDAO = new CategoriaProdutoDAO();
            categoria = tipoDAO.Consulta(id);
            return categoria;
        }
        protected override void SetTabela()
        {
            Tabela = "tbProdutos";
        }
    }
}
