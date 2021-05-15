using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class TipoUsuarioDAO : PadraoDAO<TipoUsuarioViewModel>
    {
        protected override SqlParameter[] CriaParametros(TipoUsuarioViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("Tipo", model.Tipo);
            return parametros;
        }

        protected override TipoUsuarioViewModel MontaModel(DataRow registro)
        {
            TipoUsuarioViewModel tp = new TipoUsuarioViewModel();
            tp.Id = Convert.ToInt32(registro["id"]);
            tp.Tipo = registro["Tipo"].ToString();

            return tp;
        }

        protected override void SetTabela()
        {
            Tabela = "tbTipoUsuario";
            // NomeSpListagem = "spListagemJogos";
        }
    }
}
