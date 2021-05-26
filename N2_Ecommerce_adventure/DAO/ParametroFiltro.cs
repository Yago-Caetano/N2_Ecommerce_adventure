using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N2_Ecommerce_adventure.DAO
{
    public class ParametroFiltro
    {
        public String Ordem { get; set; }
        public DateTime dataIncio { get; set; }
        public DateTime dataFim { get; set; }
        public Double PrecoInicial { get; set; } = -1;
        public Double PrecoFinal { get; set; } = -1;

    }
}
