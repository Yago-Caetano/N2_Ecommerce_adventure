using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class UsuarioDAO : PadraoDAO<UsuarioViewModel>
    {
        protected override SqlParameter[] CriaParametros(UsuarioViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[8];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("Nome", model.Nome);
            parametros[2] = new SqlParameter("Nascimento", model.Nascimento);
            parametros[3] = new SqlParameter("email", model.Email);
            parametros[4] = new SqlParameter("senha", model.senha);
            parametros[5] = new SqlParameter("cpf", model.CPF);
            parametros[6] = new SqlParameter("idTipoUsuario", model.idTipoUsuario);
            parametros[7] = new SqlParameter("statusUsuario", model.Ativado);

            return parametros;

        }

        protected override UsuarioViewModel MontaModel(DataRow registro)
        {
            UsuarioViewModel user = new UsuarioViewModel();
            user.Id = Convert.ToInt32(registro["id"]);
            user.Nome = registro["Nome"].ToString();
            user.Nascimento = Convert.ToDateTime(registro["Nascimento"]);
            user.Email = registro["email"].ToString();
            user.senha = registro["senha"].ToString();
            user.CPF = registro["cpf"].ToString();
            user.idTipoUsuario = Convert.ToInt32(registro["idTipoUsuario"]);
            user.Ativado = Convert.ToBoolean(registro["statusUsuario"]);

            if (user.idTipoUsuario > 0)
                user.Tipo_Usuario = GetUserTipo(user.idTipoUsuario);

            user.Enderecos = GetUsuariosEnderecos(user.Id);

            return user;
        }
        private TipoUsuarioViewModel GetUserTipo(int id)
        {
            TipoUsuarioViewModel tipo = new TipoUsuarioViewModel();
            TipoUsuarioDAO tipoDAO = new TipoUsuarioDAO();
            tipo = tipoDAO.Consulta(id);
            return tipo;
        }
        private List<EnderecoViewModel> GetUsuariosEnderecos(int idUsuario)
        {
            List<EnderecoViewModel> lista = new List<EnderecoViewModel>();
            EnderecoDAO endDao = new EnderecoDAO();
            var p = new SqlParameter[]
            {
                new SqlParameter("idUsuario", idUsuario),
            };
            var tabela = HelperDAO.ExecutaProcSelect("spConsultaEnderecosUsuario", p);

            foreach (DataRow table in tabela.Rows)
                lista.Add(endDao.Consulta(Convert.ToInt32(table["id"])));

            return lista;

        }

        protected override void SetTabela()
        {
            Tabela = "tbUsuario";
        }
        public override void Delete(int id)
        {
            var p = new SqlParameter[]
              {
                  new SqlParameter("idUsuario", id),
              };
            HelperDAO.ExecutaProc("spDelete_Usuario", p);
        }
        public virtual UsuarioViewModel VerificaUsuario(string login, string senha)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("login", login),
                new SqlParameter("senha", senha)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spVerificaUsuario", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }
        public UsuarioSimplificadoViewModel GetSimplifiedUser(int idUsuario)
        {
            UsuarioSimplificadoViewModel user = new UsuarioSimplificadoViewModel();
            var p = new SqlParameter[]
              {
                 new SqlParameter("idUsuario", idUsuario),
              };
            var tabela = HelperDAO.ExecutaProcSelect("spConsultaDadosUsuario", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
            {
                user.Id = Convert.ToInt32(tabela.Rows[0]["id"]);
                user.Nome = tabela.Rows[0]["Nome"].ToString();
                user.Email = tabela.Rows[0]["email"].ToString();
                user.CPF = tabela.Rows[0]["cpf"].ToString();
            }

            return user;

        }
    }
}
