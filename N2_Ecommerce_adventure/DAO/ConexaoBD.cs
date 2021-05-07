using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class ConexaoBD
    {
        public static SqlConnection GetConexao()
        {
            string strCon = "Data Source=DESKTOP-6TEUHKL\\SQLEXPRESS; Database=ExFutebol; integrated security=true";
            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
