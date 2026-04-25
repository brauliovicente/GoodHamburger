using GoodHamburger.Aplicacao.DTOs;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Command.Criar
{
    public class CriarPedidoCommand : IRequest<RespostaGeral<PedidoDto>>
    {
        public List<Guid> ItensId { get; set; }
    }
}