using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using N2_Ecommerce_adventure.Models;

namespace N2_Ecommerce_adventure.DAO
{
    public class JogadorDAO : PadraoDAO<JogadorViewModel>
    {
        protected override SqlParameter[] CriaParametros(JogadorViewModel model)
        {

            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("Nome", model.Nome);
            parametros[2] = new SqlParameter("Posicao",model.Posicao);
            parametros[3] = new SqlParameter("TimeId", model.TimeId); 

            return parametros;
        }

        protected override JogadorViewModel MontaModel(DataRow registro)
        {
             JogadorViewModel j = new JogadorViewModel();
            j.Id = Convert.ToInt32(registro["id"]);
            j.Nome = registro["nome"].ToString();
            j.Posicao = Convert.ToInt32(registro["Posicao"]);
            j.TimeId = Convert.ToInt32(registro["TimeId"]);

            return j;
        }

        protected override void SetTabela()
        {
            Tabela = "Jogadores";
            // NomeSpListagem = "spListagemJogos";
        }


    }
}
