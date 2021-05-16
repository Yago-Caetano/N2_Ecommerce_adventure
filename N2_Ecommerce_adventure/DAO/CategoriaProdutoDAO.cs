using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class CategoriaProdutoDAO : PadraoDAO<CategoriaProdutoViewModel>
    {
        protected override SqlParameter[] CriaParametros(CategoriaProdutoViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("Categoria", model.Categoria);
            return parametros;
        }

        protected override CategoriaProdutoViewModel MontaModel(DataRow registro)
        {
            CategoriaProdutoViewModel tp = new CategoriaProdutoViewModel();
            tp.Id = Convert.ToInt32(registro["id"]);
            tp.Categoria = registro["Categoria"].ToString();

            return tp;
        }

        protected override void SetTabela()
        {
            Tabela = "tbCategoriaProdutos";
        }
    }
}
