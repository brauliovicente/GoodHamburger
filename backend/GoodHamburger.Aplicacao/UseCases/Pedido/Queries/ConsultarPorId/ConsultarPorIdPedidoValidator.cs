using FluentValidation;
using GoodHamburger.Aplicacao.UseCases.Pedido.Queries.ConsultarPorId;
using GoodHamburger.Dominio.Interface;

public class ConsultarPorIdPedidoValidator : AbstractValidator<ConsultarPorIdPedidoQuery>
{
    private readonly IPedidoRepository _pedidoRepository;

    public ConsultarPorIdPedidoValidator(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;

        RuleFor(x => x.PedidoId)
            .NotEmpty()
            .WithMessage("O ID do pedido é obrigatório");

        RuleFor(x => x.PedidoId)
            .MustAsync(PedidoExisteAsync)
            .WithMessage("O pedido informado não existe");
    }

    private async Task<bool> PedidoExisteAsync(
        Guid pedidoId,
        CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ConsultarPorIdAsync(pedidoId);
        return pedido is not null;
    }
}