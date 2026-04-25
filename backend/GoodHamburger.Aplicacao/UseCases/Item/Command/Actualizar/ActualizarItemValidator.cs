using FluentValidation;
using GoodHamburger.Aplicacao.UseCases.Item.Command.Actualizar;
using GoodHamburger.Aplicacao.UseCases.Item.Command.Criar;
using GoodHamburger.Dominio.Interface;

public class ActualizarItemValidator : AbstractValidator<ActualizarItemCommand>
{
    private readonly IItemRepository _itemRepository;

    public ActualizarItemValidator(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;

        // Validação do Id
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("O identificador do item é obrigatório.")
            .MustAsync(ItemExisteAsync)
            .WithMessage("O item a ser actualizado não existe.");

        // Validação do Nome
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome do item é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome do item deve ter no máximo 100 caracteres.");

        // Validação do Preço
        RuleFor(x => x.Preco)
            .GreaterThan(0)
            .WithMessage("O preço deve ser maior que zero.");

        // Validação do Tipo
        RuleFor(x => x.Tipo)
            .IsInEnum()
            .WithMessage("Tipo de item inválido.");

        // Validação de duplicidade
        RuleFor(x => x)
            .MustAsync(ValidarItemExistenteAsync)
            .WithMessage("Já existe um item com esse nome.");
    }

    private async Task<bool> ItemExisteAsync(Guid itemId, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.ConsultarPorIdAsync(itemId);
        return item != null;
    }

    private async Task<bool> ValidarItemExistenteAsync(ActualizarItemCommand command, CancellationToken cancellationToken)
    {
        var itemExistente = await _itemRepository.ListarAsync();
        return !itemExistente.Any(i => i.Nome.Equals(command.Nome, StringComparison.OrdinalIgnoreCase) && command.ItemId!=i.Id);
    }
}