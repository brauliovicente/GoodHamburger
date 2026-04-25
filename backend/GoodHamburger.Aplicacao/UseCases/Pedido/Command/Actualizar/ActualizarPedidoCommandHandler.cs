using FluentValidation;
using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Interface;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Command.Actualizar
{
    public class ActualizarPedidoCommandHandler
        : IRequestHandler<ActualizarPedidoCommand, RespostaGeral<PedidoDto>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ActualizarPedidoCommand> _validator;

        public ActualizarPedidoCommandHandler(
            IItemRepository itemRepository,
            IPedidoRepository pedidoRepository,
            IMapper mapper, IValidator<ActualizarPedidoCommand> validator)
        {
            _itemRepository = itemRepository;
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<RespostaGeral<PedidoDto>> Handle(ActualizarPedidoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Validação
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return RespostaGeral<PedidoDto>.Falha(
                        "Erro de validação",
                        validationResult.Errors.Select(e => e.ErrorMessage).ToList());
                }

                //buscar pedido existente
                var pedidoExistente = await _pedidoRepository.ConsultarPorIdAsync(request.PedidoId);

                if (pedidoExistente is null)
                {
                    return RespostaGeral<PedidoDto>.Falha(
                        "Pedido não encontrado",
                        new List<string> { "O pedido informado não existe" });
                }

                //Buscar itens válidos
                var selecionados = await _itemRepository.ConsultarPorIdsAsync(request.ItensId);

                if (!selecionados.Any())
                {
                    return RespostaGeral<PedidoDto>.Falha(
                        "Pedido inválido",
                        new List<string> { "Nenhum item encontrado" });
                }

                pedidoExistente.SincronizarItens(selecionados);

                await _pedidoRepository.ActualizarAsync(pedidoExistente);

                //Mapear
                var dto = _mapper.Map<PedidoDto>(pedidoExistente);

                return RespostaGeral<PedidoDto>.Ok(dto, "Pedido actualizado com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<PedidoDto>.Falha(
                    "Erro ao atualizar pedido",
                    new List<string> { ex.Message });
            }
        }


    }
}