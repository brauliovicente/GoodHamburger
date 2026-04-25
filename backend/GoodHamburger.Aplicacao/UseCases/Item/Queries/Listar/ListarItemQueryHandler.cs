using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Interface;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Item.Queries.Listar
{
    public class ListarItemQueryHandler
        : IRequestHandler<ListarItemQuery, RespostaGeral<IEnumerable<ItemDto>>>
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;

        public ListarItemQueryHandler(
            IItemRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RespostaGeral<IEnumerable<ItemDto>>> Handle(
            ListarItemQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var pedidos = await _repository.ListarAsync();

                var dto = _mapper.Map<IEnumerable<ItemDto>>(pedidos);

                return RespostaGeral<IEnumerable<ItemDto>>.Ok(
                    dto,
                    "Itens listados com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<IEnumerable<ItemDto>>.Falha(
                    "Erro ao listar itens",
                    new List<string> { ex.Message });
            }
        }
    }
}