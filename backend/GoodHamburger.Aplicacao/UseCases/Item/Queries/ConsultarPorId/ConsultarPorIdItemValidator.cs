using FluentValidation;
using GoodHamburger.Aplicacao.UseCases.Item.Queries.ConsultarPorId;
using GoodHamburger.Dominio.Interface;

public class ConsultarPorIdItemValidator : AbstractValidator<ConsultarPorIdItemQuery>
{
    private readonly IItemRepository _pedidoRepository;

    public ConsultarPorIdItemValidator(IItemRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("O ID do item é obrigatório");

        RuleFor(x => x.ItemId)
            .MustAsync(ItemExisteAsync)
            .WithMessage("O item informado não existe");
    }

    private async Task<bool> ItemExisteAsync(
        Guid itemId,
        CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ConsultarPorIdAsync(itemId);
        return pedido is not null;
    }
}