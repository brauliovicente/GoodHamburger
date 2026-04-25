using FluentValidation;
using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Interface;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Command.Remover
{
    public class RemoverItemCommandHandler
        : IRequestHandler<RemoverItemCommand, RespostaGeral<ItemDto>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<RemoverItemCommand> _validator;

        public RemoverItemCommandHandler(
            IItemRepository itemRepository,
            IMapper mapper,
            IValidator<RemoverItemCommand> validator)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<RespostaGeral<ItemDto>> Handle(
            RemoverItemCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // 1. VALIDATION
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return RespostaGeral<ItemDto>.Falha(
                        "Erro de validação",
                        validationResult.Errors.Select(e => e.ErrorMessage).ToList());
                }

                var itemExistente = await _itemRepository.ConsultarPorIdAsync(request.ItemId);

                if (itemExistente == null)
                {
                    return RespostaGeral<ItemDto>.Falha(
                        "Item não encontrado",
                        new List<string> { $"Item com ID {request.ItemId} não existe" });
                }

                await _itemRepository.RemoverAsync(request.ItemId);

                var dto = _mapper.Map<ItemDto>(itemExistente);

                return RespostaGeral<ItemDto>.Ok(dto, "Item removido com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<ItemDto>.Falha(
                    "Erro ao remover item",
                    new List<string> { ex.Message });
            }
        }
    }
}