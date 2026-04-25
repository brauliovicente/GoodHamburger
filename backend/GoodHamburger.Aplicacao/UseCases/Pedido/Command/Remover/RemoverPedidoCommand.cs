using GoodHamburger.Aplicacao.DTOs;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Command.Remover
{
    public class RemoverPedidoCommand : IRequest<RespostaGeral<PedidoDto>>
    {
        public Guid PedidoId { get; set; }
    }
}