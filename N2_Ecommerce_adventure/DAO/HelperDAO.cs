using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public static class HelperDAO
    {

        #region Instruções via Procedure
        public static DataTable ExecutaProcSelect(string nomeProc, SqlParameter[] parametros)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(nomeProc, conexao))
                {
                    
                    if (parametros != null)
                        adapter.SelectCommand.Parameters.AddRange(parametros);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    conexao.Close();
                    return tabela;
                }
            }
        }

        public static object ExecuteFunction(string Nome, SqlParameter p)
        {

            SqlConnection con = ConexaoBD.GetConexao();
            SqlCommand com = new SqlCommand("SELECT dbo."+ Nome + "(1)", con);
            //com.Parameters.Add(p);
            return com.ExecuteScalar();
        }

        public static DataTable ExecutaProcSelect(string nomeProc, SqlParameter[] parametros, SqlConnection conexao)
        {

                using (SqlDataAdapter adapter = new SqlDataAdapter(nomeProc, conexao))
                {

                    if (parametros != null)
                        adapter.SelectCommand.Parameters.AddRange(parametros);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    conexao.Close();
                    return tabela;
                }
            
        }


        public static int ExecutaProc(string nomeProc, SqlParameter[] parametros,bool consultaUltimoIdentity = false)
        {
            int retValue = 0;
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlCommand comando = new SqlCommand(nomeProc, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    if (parametros != null)
                        comando.Parameters.AddRange(parametros);
                    comando.ExecuteNonQuery();

                    if(consultaUltimoIdentity)
                    {
                        var data = ExecutaProcSelect("spGetIdentity",null,conexao);
                        retValue = Convert.ToInt32(data.Rows[0]["id"]);
                    }
                }
                conexao.Close();
                return retValue;
            }
        }

        #endregion
    }
}
