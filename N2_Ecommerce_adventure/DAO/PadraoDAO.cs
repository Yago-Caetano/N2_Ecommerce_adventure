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

        public virtual void Insert(T model)
        {
            HelperDAO.ExecutaProc("spInsert_" + Tabela, CriaParametros(model));
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

        public virtual T Filtro(T model, ParametroFiltro parametros)
        {
            var p = CriaParametrosFiltro(model, parametros);

            var tabela = HelperDAO.ExecutaProcSelect("spFiltro_" + Tabela, p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
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

        protected virtual SqlParameter[] CriaParametrosFiltro(T model, ParametroFiltro parametros)
        {
            var p = CriaParametros(model);
            
            if(parametros.dataIncio != null)
            {
                p.Append(new SqlParameter("dataInicio", parametros.dataIncio));
            }

            if (parametros.dataFim != null)
            {
                p.Append(new SqlParameter("dataFim", parametros.dataFim));
            }

            if (parametros.PrecoFinal != -1)
            {
                p.Append(new SqlParameter("PrecoFinal", parametros.PrecoFinal));
            }

            if (parametros.PrecoInicial != -1)
            {
                p.Append(new SqlParameter("PrecoInicial", parametros.PrecoInicial));
            }
            return p;
        }

    }
}
