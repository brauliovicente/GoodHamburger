using GoodHamburger.Dominio.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Aplicacao.DTOs
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco {  get; set; }
        public Tipo Tipo { get; set; }
    }
}
