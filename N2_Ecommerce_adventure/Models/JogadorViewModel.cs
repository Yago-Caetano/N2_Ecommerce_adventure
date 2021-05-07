using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.Models
{
    public class JogadorViewModel : PadraoViewModel
    {
        public enum Posicoes { Goleiro = 1, Atacante, Meia, Zagueiro, Lateral }
        public string Nome { get; set; }
        public int Posicao { get; set; }
        public int TimeId { get; set; }
    }
}
