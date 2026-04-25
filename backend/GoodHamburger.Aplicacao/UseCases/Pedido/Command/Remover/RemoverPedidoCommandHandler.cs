using FluentValidation;
using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Interface;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Command.Remover
{
    public class RemoverPedidoCommandHandler
        : IRequestHandler<RemoverPedidoCommand, RespostaGeral<PedidoDto>>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RemoverPedidoCommand> _validator;

        public RemoverPedidoCommandHandler(
            IPedidoRepository pedidoRepository,
            IMapper mapper,
            IValidator<RemoverPedidoCommand> validator)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<RespostaGeral<PedidoDto>> Handle(RemoverPedidoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return RespostaGeral<PedidoDto>.Falha(
                        "Erro de validação",
                        validationResult.Errors.Select(e => e.ErrorMessage).ToList());
                }

                var pedido = await _pedidoRepository.ConsultarPorIdAsync(request.PedidoId);
                await _pedidoRepository.RemoveAsync(request.PedidoId);

                var dto = _mapper.Map<PedidoDto>(pedido);

                return RespostaGeral<PedidoDto>.Ok(dto, "Pedido removido com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<PedidoDto>.Falha(
                    "Erro ao remover pedido",
                    new List<string> { ex.Message });
            }
        }
    }
}