using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class ConexaoBD
    {
        private static string servername = @"DESKTOP-G71U68M\WINCCPLUSMIG2014";
        private static string database = "N2_dudu_Viotti_ecommerce";
        private static string server_login = "sa";
        private static string server_password = "123456";
        public static SqlConnection GetConexao()
        {
            //string strCon = $"Data Source={servername};Initial Catalog={database};User ID={server_login};Password={server_password}";
            string strCon = "Data Source=DESKTOP-6TEUHKL\\SQLEXPRESS; Database=N2_dudu_Viotti_ecommerce; integrated security=true";
            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
