using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using N2_Ecommerce_adventure.Models;

namespace N2_Ecommerce_adventure.DAO
{
    public abstract class PadraoDAO<T> where T : PadraoViewModel
    {
        protected PadraoDAO()
        {
            SetTabela();
        }

        protected string Tabela { get; set; }
        protected string NomeSpListagem { get; set; } = "spListagem";
        protected string Chave { get; set; } = "id"; // valor default
        protected abstract SqlParameter[] CriaParametros(T model);

        protected abstract T MontaModel(DataRow registro);
        protected abstract void SetTabela();

        public virtual int Insert(T model,bool getId=false)
        {
            return HelperDAO.ExecutaProc("spInsert_" + Tabela, CriaParametros(model),getId);

        }

        public virtual void Update(T model)
        {
            HelperDAO.ExecutaProc("spUpdate_" + Tabela, CriaParametros(model));
        }

        public virtual void Delete(int id)
        {
            var p = new SqlParameter[]
            {
                 new SqlParameter("id", id),
                 new SqlParameter("tabela", Tabela)
            };
            HelperDAO.ExecutaProc("spDelete", p);
        }

        public virtual T Consulta(int id)
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


        public virtual List<T> ListagemView(String nomeView)
        {
            List<T> returnList = new List<T>();
            DataTable table = new DataTable();

            using (var connection = ConexaoBD.GetConexao())
            {

                /* Como requisito do professor Viotti, houve a necessidade de criar uma view para consulta
                    Por isso, nesse caso tivemos que escrever a query de consulta
                 */
                using (var command = new SqlCommand("SELECT * FROM " + nomeView, connection))
                {
                    // Loads the query results into the table
                    table.Load(command.ExecuteReader());

                    foreach (DataRow reg in table.Rows)
                    {
                        returnList.Add(MontaModel(reg));
                    }
                }

                connection.Close();
            }
            return returnList;
        }

        public virtual List<T> Filtro(T model, SqlParameter[] parametrosAdcionais)
        {
            SqlParameter[] paramsAux = CriaParametros(model);
            
            foreach(SqlParameter sql in parametrosAdcionais)
            {
                paramsAux.Append(sql);
            }

            return Filtro(paramsAux);
        }

        public virtual List<T> Filtro(SqlParameter[] parameters)
        {
            List<T> returnList = new List<T>();
            var tabela = HelperDAO.ExecutaProcSelect("spFiltro_" + Tabela, parameters);
            if (tabela.Rows.Count == 0)
                return null;
            else
            {
                foreach(DataRow reg in tabela.Rows)
                {
                    returnList.Add(MontaModel(reg));
                }
                return returnList;
            }
                
        }



        public virtual int ProximoId()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spProximoId", p);
            return Convert.ToInt32(tabela.Rows[0][0]);
        }

        public virtual List<T> Listagem()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", Tabela),
                new SqlParameter("Ordem", "1") // 1 é o primeiro campo da tabela,
                                // ou seja, a chave primária
            };
            var tabela = HelperDAO.ExecutaProcSelect(NomeSpListagem, p);
            List<T> lista = new List<T>();
            foreach (DataRow registro in tabela.Rows)
            {
                lista.Add(MontaModel(registro));
            }

            return lista;
        }

       
    }
}
