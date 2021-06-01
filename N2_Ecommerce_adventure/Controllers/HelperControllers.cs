using Microsoft.AspNetCore.Http;
using N2_Ecommerce_adventure.DAO;
using N2_Ecommerce_adventure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Controllers
{
    public class HelperControllers
    {
        public static Boolean VerificaUserLogado(ISession session)
        {
            string logado = session.GetString("Logado");
            if (logado == null)
                return false;
            else
                return true;
        }
        public static int GetUserLogadoID(ISession session)
        {
            string logado = session.GetString("Logado");
            if (logado == null)
                return -1;
            else
                return Convert.ToInt32(logado);
        }


        public static void LimparCarrinho(ISession session)
        {
            session.Remove("Carrinho");
        }

        public static List<CategoriaProdutoViewModel> CarregaCategoriasCabecalho()
        {
            //verifica as categorias cadastradas
            CategoriaProdutoDAO mDAO = new CategoriaProdutoDAO();
            var categorias = mDAO.Listagem();
            return categorias;
        }

    }
}
