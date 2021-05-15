using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class EnderecoDAO:   PadraoDAO<EnderecoViewModel>
    {
        protected override SqlParameter[] CriaParametros(EnderecoViewModel model)
        {

            SqlParameter[] parametros = new SqlParameter[7];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("Rua", model.Rua);
            parametros[2] = new SqlParameter("Complemento", model.Complemento);
            parametros[3] = new SqlParameter("numero", model.Numero);
            parametros[4] = new SqlParameter("Cep", model.CEP);
            parametros[5] = new SqlParameter("Cidade", model.Cidade);
            parametros[6] = new SqlParameter("statusEnd", model.Ativo);

            return parametros;
        }

        protected override EnderecoViewModel MontaModel(DataRow registro)
        {
            EnderecoViewModel e = new EnderecoViewModel();
            e.Id = Convert.ToInt32(registro["id"]);
            e.Rua = registro["Rua"].ToString();
            e.Complemento = registro["Complemento"].ToString();
            e.Numero = Convert.ToInt32(registro["numero"]);
            e.CEP = registro["Cep"].ToString();
            e.Cidade = registro["Cidade"].ToString();
            e.Ativo = Convert.ToBoolean(registro["statusEnd"]);

            return e;

        }
        public void InsertEndereco(EnderecoViewModel model,int idUsuario)
        {
            base.Insert(model);
            var p = new SqlParameter[]
            {
                 new SqlParameter("idUsuario",idUsuario ),
                 new SqlParameter("idEndereco",model.Id )
            };
            HelperDAO.ExecutaProc("spInsertUserEndereco" , p);
        }
        public override void Delete(int id)
        {
            var p = new SqlParameter[]
            {
                 new SqlParameter("idEndereco", id),
            };
            HelperDAO.ExecutaProc("spDelete_Enderecos", p);
        }

        protected override void SetTabela()
        {
            Tabela = "tbEnderecos";
            // NomeSpListagem = "spListagemJogos";
        }

    }
}
