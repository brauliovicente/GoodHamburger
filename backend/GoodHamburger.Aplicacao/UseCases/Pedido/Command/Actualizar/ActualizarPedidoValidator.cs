using FluentValidation;
using GoodHamburger.Aplicacao.UseCases.Pedido.Command.Actualizar;
using GoodHamburger.Dominio.Interface;
using GoodHamburger.Dominio.Enum;

public class ActualizarPedidoValidator : AbstractValidator<ActualizarPedidoCommand>
{
    private readonly IItemRepository _itemRepository;
    private readonly IPedidoRepository _pedidoRepository;

    public ActualizarPedidoValidator(
        IItemRepository itemRepository,
        IPedidoRepository pedidoRepository)
    {
        _itemRepository = itemRepository;
        _pedidoRepository = pedidoRepository;

        // Pedido
        RuleFor(x => x.PedidoId)
            .NotEmpty()
            .WithMessage("O ID do pedido é obrigatório")
            .MustAsync(PedidoExisteAsync)
            .WithMessage("Recurso não encontrado");

        // Itens
        RuleFor(x => x.ItensId)
            .NotNull()
            .WithMessage("Itens não podem ser nulos")
            .NotEmpty()
            .WithMessage("O pedido deve conter pelo menos um item")
            .Must(ValidarItensDuplicados)
            .WithMessage("Itens Duplicados: Não é permitido itens duplicados no pedido")
            .DependentRules(() =>
            {
                RuleFor(x => x)
                    .MustAsync(TodosItensExistemAsync)
                    .WithMessage("Recurso indisponível: Um ou mais itens não existem no cardápio");

                RuleFor(x => x)
                    .MustAsync(ValidarRegrasNegocioAsync)
                    .WithMessage("O pedido deve conter no máximo 1 sanduíche, 1 batata e 1 refrigerante");
            });
    }

    // Pedido existe
    private async Task<bool> PedidoExisteAsync(
        Guid pedidoId,
        CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ConsultarPorIdAsync(pedidoId);
        return pedido != null;
    }

    // Duplicados
    private bool ValidarItensDuplicados(List<Guid> itensId)
        => itensId.Distinct().Count() == itensId.Count;


    // Existência dos itens
    private async Task<bool> TodosItensExistemAsync(
        ActualizarPedidoCommand command,
        CancellationToken cancellationToken)
    {
        var idsUnicos = command.ItensId.Distinct().ToList();

        var itens = await _itemRepository.ConsultarPorIdsAsync(idsUnicos);

        return itens.Count == idsUnicos.Count;
    }

    // Regras de negócio
    private async Task<bool> ValidarRegrasNegocioAsync(
        ActualizarPedidoCommand command,
        CancellationToken cancellationToken)
    {
        var itens = await _itemRepository.ConsultarPorIdsAsync(command.ItensId.Distinct().ToList());

        var sanduiches = itens.Count(i => i.Tipo == Tipo.Sanduiche);
        var batatas = itens.Count(i => i.Tipo == Tipo.Batata);
        var refrigerantes = itens.Count(i => i.Tipo == Tipo.Refrigerante);

        return sanduiches <= 1 &&
               batatas <= 1 &&
               refrigerantes <= 1;
    }
}