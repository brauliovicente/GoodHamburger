using FluentValidation;
using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Entidade;
using GoodHamburger.Dominio.Interface;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Command.Actualizar
{
    public class ActualizarItemCommandHandler
        : IRequestHandler<ActualizarItemCommand, RespostaGeral<ItemDto>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ActualizarItemCommand> _validator;

        public ActualizarItemCommandHandler(
            IItemRepository itemRepository,
            IMapper mapper,
            IValidator<ActualizarItemCommand> validator)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<RespostaGeral<ItemDto>> Handle(
            ActualizarItemCommand request,
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

                // 2. BUSCAR ITEM EXISTENTE
                var itemExistente = await _itemRepository.ConsultarPorIdAsync(request.ItemId);

                itemExistente.SetNome(request.Nome);
                itemExistente.SetTipo(request.Tipo);
                itemExistente.SetPreco(request.Preco);

                await _itemRepository.ActualizarAsync(itemExistente);
                var dto = _mapper.Map<ItemDto>(itemExistente);

                return RespostaGeral<ItemDto>.Ok(dto, "Item atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<ItemDto>.Falha(
                    "Erro ao atualizar item",
                    new List<string> { ex.Message });
            }
        }
    }
}