using FluentValidation;
using GoodHamburger.Aplicacao.UseCases.Item.Command.Criar;
using GoodHamburger.Dominio.Interface;
using GoodHamburger.Dominio.Enum;
using System.Linq;

public class CriarItemValidator : AbstractValidator<CriarItemCommand>
{
    private readonly IItemRepository _itemRepository;

    public CriarItemValidator(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;

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

        RuleFor(x => x)
            .MustAsync(ValidarItemExistenteAsync)
            .WithMessage("Já existe um item com esse nome.");
    }

    // Verifica se o nome do item já existe no banco
    private async Task<bool> ValidarItemExistenteAsync(CriarItemCommand command, CancellationToken cancellationToken)
    {
        var itemExistente = await _itemRepository.ListarAsync();
        return !itemExistente.Any(i => i.Nome.Equals(command.Nome, StringComparison.OrdinalIgnoreCase));
    }
    
}