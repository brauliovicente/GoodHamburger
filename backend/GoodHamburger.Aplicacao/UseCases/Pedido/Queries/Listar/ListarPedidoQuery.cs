using GoodHamburger.Aplicacao.DTOs;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Queries.Listar
{
    public class ListarPedidoQuery : IRequest<RespostaGeral<IEnumerable<PedidoDto>>>
    {}
}