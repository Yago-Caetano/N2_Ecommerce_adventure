using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class PedidosDAO : PadraoDAO<PedidosViewModel>
    {
        public enum Model { Completo, Informacaoes };
        protected override SqlParameter[] CriaParametros(PedidosViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("idStatus", model.status.Id);
            parametros[2] = new SqlParameter("idUsuario", model.Cliente.Id);
            parametros[3] = new SqlParameter("idEndereco", model.endereco.Id);
            parametros[4] = new SqlParameter("data", model.data);

            return parametros;
        }

        protected PedidosViewModel MontaModel(DataRow registro, Model model)
        {
            PedidosViewModel e = new PedidosViewModel();
            e.Id = Convert.ToInt32(registro["id"]);
            e.status.Id = Convert.ToInt32(registro["idStatus"]);
            e.Cliente.Id = Convert.ToInt32(registro["idUsuario"]);
            e.endereco.Id = Convert.ToInt32(registro["idUsuario"]);
            e.data = Convert.ToDateTime(registro["data"]);
            MontaModelo(e, model);
            return e;
        }
        public virtual List<PedidosViewModel> Listar(Model model)
        {
            var p = new SqlParameter[]
           {
                new SqlParameter("tabela", Tabela),
                new SqlParameter("Ordem", "1") // 1 é o primeiro campo da tabela,
                                               // ou seja, a chave primária
           };
            var tabela = HelperDAO.ExecutaProcSelect(NomeSpListagem, p);
            List<PedidosViewModel> lista = new List<PedidosViewModel>();
            foreach (DataRow registro in tabela.Rows)
            {
                lista.Add(MontaModel(registro, model));
            }
            return lista;

        }
        public PedidosViewModel MontaModelo(PedidosViewModel pedido, Model model)
        {
            if (model == Model.Informacaoes)
            {
                pedido.Cliente = GetUsuario(pedido);
                pedido.Cliente = GetUsuario(pedido);
                pedido.status = GetStatusPedido(pedido);
                pedido.endereco = GetEndereco(pedido);
            }
            else if (model == Model.Completo)
            {
                pedido.Cliente = GetUsuario(pedido);
                pedido.status = GetStatusPedido(pedido);
                pedido.endereco = GetEndereco(pedido);
                pedido.Itens = GetListaItens(pedido);
            }
            return pedido;
        }

        protected PedidosViewModel MontaModelConsulta(DataRow registro)
        {
            PedidosViewModel e = new PedidosViewModel();
            e.Id = Convert.ToInt32(registro["id"]);
            e.Cliente.Nome = (registro["Nome"].ToString());
            e.endereco.Cidade = (registro["Cidade"].ToString());
            e.endereco.CEP = (registro["CEP"].ToString());
            e.Valor = Convert.ToDouble(registro["Valor"]);
            e.data = Convert.ToDateTime(registro["data"]);
            return e;
        }

        public virtual PedidosViewModel Consulta(int id, Model model)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spConsulta", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0], model);
        }

        public List<PedidosViewModel> GetAll(int statusPedido)
        {
            List<PedidosViewModel> returnList = new List<PedidosViewModel>();

            var p = new SqlParameter[] { new SqlParameter("idstatus", statusPedido) };
            var tabela  = HelperDAO.ExecutaProcSelect("fnc_GetAllPedidos", p);

            foreach (DataRow registro in tabela.Rows)
            {
                 returnList.Add(MontaModelConsulta(registro));
            }
            return returnList;


        }

        private UsuarioSimplificadoViewModel GetUsuario(PedidosViewModel pedido)
        {
            UsuarioDAO dao = new UsuarioDAO();
            return dao.GetSimplifiedUser(pedido.Cliente.Id);
        }
        private StatusPedidoViewModel GetStatusPedido(PedidosViewModel pedido)
        {
            StatusPedidoDAO dao = new StatusPedidoDAO();
            return dao.Consulta(pedido.status.Id);
        }
        private EnderecoViewModel GetEndereco(PedidosViewModel pedido)
        {
            EnderecoDAO dao = new EnderecoDAO();
            return dao.Consulta(pedido.endereco.Id);
        }
        private List<ProdutoPedidoViewModel> GetListaItens(PedidosViewModel pedido)
        {
            ProdutoPedidoDAO dao = new ProdutoPedidoDAO();
            return dao.Listagem(pedido.Id);
        }

        protected override void SetTabela()
        {
            Tabela = "tbPedidos";
        }

        protected override PedidosViewModel MontaModel(DataRow registro)
        {
            return MontaModel(registro, Model.Completo);
        }

    }
}
