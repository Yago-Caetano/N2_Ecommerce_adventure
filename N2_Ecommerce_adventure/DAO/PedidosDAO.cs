using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class PedidosDAO : PadraoDAO<PedidosViewModel>
    {
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

        protected override PedidosViewModel MontaModel(DataRow registro)
        {
            PedidosViewModel e = new PedidosViewModel();
            e.Id = Convert.ToInt32(registro["id"]);
            e.status.Id = Convert.ToInt32(registro["idStatus"]);
            e.Cliente.Id = Convert.ToInt32(registro["idUsuario"]);
            e.endereco.Id = Convert.ToInt32(registro["idUsuario"]);
            e.data = Convert.ToDateTime(registro["data"]);
            return e;
        }
        public PedidosViewModel MontaModelCompleto(PedidosViewModel pedido)
        {
            pedido.Cliente = GetUsuario(pedido);
            pedido.status = GetStatusPedido(pedido);
            pedido.endereco = GetEndereco(pedido);
            return pedido;
        }
        private UsuarioSimplificadoViewModel GetUsuario(PedidosViewModel pedido)
        {
            UsuarioSimplificadoViewModel user = new UsuarioSimplificadoViewModel();
            UsuarioDAO dao = new UsuarioDAO();
            return user = dao.GetSimplifiedUser(pedido.Cliente.Id);
        }
        private StatusPedidoViewModel GetStatusPedido(PedidosViewModel pedido)
        {
            StatusPedidoViewModel p = new StatusPedidoViewModel();
            StatusPedidoDAO dao = new StatusPedidoDAO();
            return p = dao.Consulta(pedido.status.Id);
        }
        private EnderecoViewModel GetEndereco(PedidosViewModel pedido)
        {
            EnderecoViewModel p = new EnderecoViewModel();
            EnderecoDAO dao = new EnderecoDAO();
            return p = dao.Consulta(pedido.endereco.Id);
        }

        protected override void SetTabela()
        {
            throw new NotImplementedException();
        }
    }
}
