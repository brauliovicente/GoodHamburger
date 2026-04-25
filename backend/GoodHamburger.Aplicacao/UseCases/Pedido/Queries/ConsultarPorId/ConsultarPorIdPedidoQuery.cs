using GoodHamburger.Aplicacao.DTOs;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Queries.ConsultarPorId
{
    public class ConsultarPorIdPedidoQuery : IRequest<RespostaGeral<PedidoDto>>
    {
        public Guid PedidoId { get; set; }
    }
}