using FluentValidation;
using GoodHamburger.Aplicacao.UseCases.Item.Command.Remover;
using GoodHamburger.Dominio.Interface;

public class RemoverItemValidator : AbstractValidator<RemoverItemCommand>
{
    private readonly IItemRepository _itemRepository;

    public RemoverItemValidator(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;

        // Validação do Id
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("O identificador do item é obrigatório.")
            .MustAsync(ItemExisteAsync)
            .WithMessage("O item a ser actualizado não existe.");

    }

    private async Task<bool> ItemExisteAsync(Guid itemId, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.ConsultarPorIdAsync(itemId);
        return item != null;
    }
}