using FluentValidation;
using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Interface;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Queries.ConsultarPorId
{
    public class ConsultarPorIdPedidoQueryHandler
        : IRequestHandler<ConsultarPorIdPedidoQuery, RespostaGeral<PedidoDto>>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ConsultarPorIdPedidoQuery> _validator;

        public ConsultarPorIdPedidoQueryHandler(
            IPedidoRepository pedidoRepository,
            IMapper mapper,
            IValidator<ConsultarPorIdPedidoQuery> validator)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<RespostaGeral<PedidoDto>> Handle(
            ConsultarPorIdPedidoQuery request,
            CancellationToken cancellationToken)
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

                var dto = _mapper.Map<PedidoDto>(pedido);

                return RespostaGeral<PedidoDto>.Ok(dto, "Pedido consultado com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<PedidoDto>.Falha(
                    "Erro ao consultar pedido",
                    new List<string> { ex.Message });
            }
        }
    }
}