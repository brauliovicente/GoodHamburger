using GoodHamburger.Aplicacao.DTOs;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Command.Actualizar
{
    public class ActualizarPedidoCommand : IRequest<RespostaGeral<PedidoDto>>
    {
        public Guid PedidoId { get; set; }
        public List<Guid> ItensId { get; set; } = new();
    }
}