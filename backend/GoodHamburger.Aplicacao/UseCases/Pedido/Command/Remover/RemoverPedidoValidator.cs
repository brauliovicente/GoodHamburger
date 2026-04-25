using FluentValidation;
using GoodHamburger.Aplicacao.UseCases.Pedido.Command.Remover;
using GoodHamburger.Dominio.Interface;
using GoodHamburger.Dominio.Enum;
using GoodHamburger.Infraestrutura.Persistence.Repository;

public class RemoverPedidoValidator : AbstractValidator<RemoverPedidoCommand>
{
    private readonly IItemRepository _itemRepository;
    private readonly IPedidoRepository _pedidoRepository;

    public RemoverPedidoValidator(IItemRepository itemRepository, IPedidoRepository pedidoRepository)
    {
        _itemRepository = itemRepository;
        _pedidoRepository = pedidoRepository;

        RuleFor(x => x.PedidoId)
            .NotEmpty()
            .WithMessage("O ID do pedido é obrigatório");

        RuleFor(x => x.PedidoId)
            .MustAsync(PedidoExisteAsync)
            .WithMessage("O pedido informado não existe");

      
    }
    
    private async Task<bool> PedidoExisteAsync(Guid pedidoId, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ConsultarPorIdAsync(pedidoId);
        return pedido != null;
    }
}