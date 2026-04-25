using GoodHamburger.Aplicacao.DTOs;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Queries.Listar
{
    public class ListarItemQuery : IRequest<RespostaGeral<IEnumerable<ItemDto>>>
    {}
}