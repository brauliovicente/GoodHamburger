using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Enum;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Command.Remover
{
    public class RemoverItemCommand : IRequest<RespostaGeral<ItemDto>>
    {
        public Guid ItemId { get; set; }
    }
}