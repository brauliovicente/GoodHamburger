using GoodHamburger.Aplicacao.DTOs;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Queries.ConsultarPorId
{
    public class ConsultarPorIdItemQuery : IRequest<RespostaGeral<ItemDto>>
    {
        public Guid ItemId { get; set; }
    }
}