using FluentValidation;
using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Interface;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Queries.ConsultarPorId
{
    public class ConsultarPorIdItemQueryHandler
        : IRequestHandler<ConsultarPorIdItemQuery, RespostaGeral<ItemDto>>
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<ConsultarPorIdItemQuery> _validator;

        public ConsultarPorIdItemQueryHandler(
            IItemRepository repository,
            IMapper mapper,
            IValidator<ConsultarPorIdItemQuery> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<RespostaGeral<ItemDto>> Handle(
            ConsultarPorIdItemQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return RespostaGeral<ItemDto>.Falha(
                        "Erro de validação",
                        validationResult.Errors.Select(e => e.ErrorMessage).ToList());
                }

                var pedido = await _repository.ConsultarPorIdAsync(request.ItemId);

                var dto = _mapper.Map<ItemDto>(pedido);

                return RespostaGeral<ItemDto>.Ok(dto, "Item consultado com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<ItemDto>.Falha(
                    "Erro ao consultar pedido",
                    new List<string> { ex.Message });
            }
        }
    }
}