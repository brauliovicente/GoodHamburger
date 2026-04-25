using FluentValidation;
using GoodHamburger.Aplicacao.UseCases.Pedido.Command.Criar;
using GoodHamburger.Dominio.Interface;
using GoodHamburger.Dominio.Enum;

public class CriarPedidoValidator : AbstractValidator<CriarPedidoCommand>
{
    private readonly IItemRepository _itemRepository;

    public CriarPedidoValidator(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;

        RuleFor(x => x.ItensId)
        .NotNull()
        .WithMessage("A lista de itens não pode ser nula")
        .NotEmpty()
        .WithMessage("O pedido deve conter pelo menos um item")
        .Must(ValidarItensDuplicados)
        .WithMessage("Itens Duplicados: Não é permitido itens duplicados no pedido")
        .DependentRules(() =>
        {
            RuleFor(x => x)
                .MustAsync(TodosItensExistemAsync)
                .WithMessage("Recurso indisponível: Um ou mais itens solicitados não existem no cardápio");

            RuleFor(x => x)
                .MustAsync(ValidarRegrasNegocioAsync)
                .WithMessage("O pedido deve conter no máximo 1 sanduíche, 1 batata e 1 refrigerante");
        });


    }

    //Evita duplicados
    private bool ValidarItensDuplicados(List<Guid> itensId)
    {
        return itensId.Distinct().Count() == itensId.Count;
    }

    //Garante que todos os IDs existem
    private async Task<bool> TodosItensExistemAsync(
    CriarPedidoCommand command,
    CancellationToken cancellationToken)
    {
        var idsUnicos = command.ItensId.Distinct().ToList();

        var itens = await _itemRepository.ConsultarPorIdsAsync(idsUnicos);

        return itens.Count == idsUnicos.Count;
    }

    //Regras de negócio principais
    private async Task<bool> ValidarRegrasNegocioAsync(
        CriarPedidoCommand command,
        CancellationToken cancellationToken)
    {
        var itens = await _itemRepository.ConsultarPorIdsAsync(command.ItensId);

        var sanduiches = itens.Count(i => i.Tipo == Tipo.Sanduiche);
        var batatas = itens.Count(i => i.Tipo == Tipo.Batata);
        var refrigerantes = itens.Count(i => i.Tipo == Tipo.Refrigerante);

        return sanduiches <= 1 &&
               batatas <= 1 &&
               refrigerantes <= 1;
    }
}