using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Dominio.Entidade
{
    public class PaginacaoResultado<T>
    {
        public IEnumerable<T> Dados { get; set; }
        public int TotalRegistos { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
    }
}
