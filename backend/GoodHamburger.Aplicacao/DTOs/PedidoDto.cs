using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Aplicacao.DTOs
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public List<ItemDto> Items { get; set; }

        public decimal Total { get; set; }
        public decimal SubTotal {  get; set; }
    }
}
