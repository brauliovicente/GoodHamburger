using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Interface;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Queries.Listar
{
    public class ListarPedidoQueryHandler
        : IRequestHandler<ListarPedidoQuery, RespostaGeral<IEnumerable<PedidoDto>>>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public ListarPedidoQueryHandler(
            IPedidoRepository pedidoRepository,
            IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }

        public async Task<RespostaGeral<IEnumerable<PedidoDto>>> Handle(
            ListarPedidoQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var pedidos = await _pedidoRepository.ListarAsync();

                var dto = _mapper.Map<IEnumerable<PedidoDto>>(pedidos);

                return RespostaGeral<IEnumerable<PedidoDto>>.Ok(
                    dto,
                    "Pedidos listados com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<IEnumerable<PedidoDto>>.Falha(
                    "Erro ao listar pedidos",
                    new List<string> { ex.Message });
            }
        }
    }
}