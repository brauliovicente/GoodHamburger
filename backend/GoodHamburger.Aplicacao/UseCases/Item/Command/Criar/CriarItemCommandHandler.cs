using FluentValidation;
using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Entidade;
using GoodHamburger.Dominio.Interface;
using Mapster;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Command.Criar
{
    public class CriarItemCommandHandler
        : IRequestHandler<CriarItemCommand, RespostaGeral<ItemDto>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CriarItemCommand> _validator;

        public CriarItemCommandHandler(
            IItemRepository itemRepository,
            IMapper mapper,
            IValidator<CriarItemCommand> validator)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<RespostaGeral<ItemDto>> Handle(CriarItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validação do comando
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return RespostaGeral<ItemDto>.Falha(
                        "Erro de validação",
                        validationResult.Errors.Select(e => e.ErrorMessage).ToList());
                }

                var item = new Dominio.Entidade.Item(request.Nome, request.Preco, request.Tipo);
                await _itemRepository.CriarAsync(item);

                var dto = _mapper.Map<ItemDto>(item);

                return RespostaGeral<ItemDto>.Ok(dto, "Item criado com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<ItemDto>.Falha(
                    "Erro ao criar item",
                    new List<string> { ex.Message });
            }
        }
    }
}