using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class EstoqueDAO : PadraoDAO<EstoqueModel>
    {
        protected override SqlParameter[] CriaParametros(EstoqueModel model)
        {
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("Nome", model.Nome);
            parametros[1] = new SqlParameter("Quantidade", model.Quantidade);
            parametros[2] = new SqlParameter("QuantidadeEmOrdem", model.QuantidadeEmOrdem);

            return parametros;
        }

        protected override EstoqueModel MontaModel(DataRow registro)
        {
            EstoqueModel estoque = new EstoqueModel();
            estoque.Nome = registro["Nome"].ToString();
            estoque.Quantidade = Convert.ToInt32(registro["Quantidade"]);
            estoque.QuantidadeEmOrdem = Convert.ToInt32(registro["QuantidadeEmOrdem"]);
            return estoque;
        }

        protected override void SetTabela()
        {
            Tabela = "Estoque";
        }
    }
}
