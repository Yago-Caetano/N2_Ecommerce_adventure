using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Models
{
    public class ProdutosViewModel:PadraoViewModel
    {
        public string Nome { get; set; }
        public double Preço { get; set; }
        public string Descricao { get; set; }
        public int idCategoria { get; set; }

        public CategoriaProdutoViewModel Categoria_Produto { get; set; }
        /// <summary>
        /// Imagem recebida do form pelo controller
        /// </summary>
        public IFormFile Foto { get; set; }
        public byte[] FotoEmByte { get; set; }
        /// <summary>
        /// Imagem usada para ser enviada ao form no formato para ser exibida
        /// </summary>
        public string FotoEmBase64
        {
            get
            {
                if (FotoEmByte != null)
                    return Convert.ToBase64String(FotoEmByte);
                else
                    return string.Empty;
            }
        }
    }
}
