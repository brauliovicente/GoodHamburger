using FluentValidation;
using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Entidade;
using GoodHamburger.Dominio.Enum;
using GoodHamburger.Dominio.Interface;
using Mapster;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Command.Criar
{
    public class CriarPedidoCommandHandler
        : IRequestHandler<CriarPedidoCommand, RespostaGeral<PedidoDto>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CriarPedidoCommand> _validator;

        public CriarPedidoCommandHandler(
            IItemRepository itemRepository,
            IPedidoRepository pedidoRepository,
            IMapper mapper, IValidator<CriarPedidoCommand> validator)
        {
            _itemRepository = itemRepository;
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
            _validator = validator; 
        }

        public async Task<RespostaGeral<PedidoDto>> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
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

                var itens = await _itemRepository.ListarAsync();

                var selecionados = itens
                    .Where(i => request.ItensId.Contains(i.Id))
                    .ToList();

                if (!selecionados.Any())
                {
                    return RespostaGeral<PedidoDto>.Falha(
                        "Pedido inválido",
                        new List<string> { "Nenhum item encontrado" });
                }

                var pedido = new Dominio.Entidade.Pedido();

                foreach (var item in selecionados)
                    pedido.AdicionarItem(item);

                await _pedidoRepository.CriarAsync(pedido);

                var dto = _mapper.Map<PedidoDto>(pedido);

                return RespostaGeral<PedidoDto>.Ok(dto, "Pedido criado com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<PedidoDto>.Falha(
                    "Erro ao criar pedido",
                    new List<string> { ex.Message });
            }
        }
    }
}