using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class StatusPedidoDAO 
    {
        public StatusPedidoDAO()
        {
            Tabela = "tbStatusPedido";
        }

        protected string Tabela { get; set; }
        public virtual List<StatusPedidoViewModel> Listagem()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", Tabela),
                new SqlParameter("Ordem", "1") // 1 é o primeiro campo da tabela,
                                // ou seja, a chave primária
            };
            var tabela = HelperDAO.ExecutaProcSelect("spListagem", p);
            List<StatusPedidoViewModel> lista = new List<StatusPedidoViewModel>();
            foreach (DataRow registro in tabela.Rows)
            {
                lista.Add(MontaModel(registro));
            }
            return lista;
        }
        public virtual StatusPedidoViewModel Consulta(int id)
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
                return MontaModel(tabela.Rows[0]);
        }
        protected  StatusPedidoViewModel MontaModel(DataRow registro)
        {
            StatusPedidoViewModel statusPedido = new StatusPedidoViewModel();
            statusPedido.Id = Convert.ToInt32(registro["id"]);
            statusPedido.status = registro["PedidoStatus"].ToString();
           
            return statusPedido;
        }
    }
}
