using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Enum;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Command.Actualizar
{
    public class ActualizarItemCommand : IRequest<RespostaGeral<ItemDto>>
    {
        public Guid ItemId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public Tipo Tipo { get; set; }
    }
}