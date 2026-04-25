using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Entidade;
using GoodHamburger.Dominio.Interface;
using MapsterMapper;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Queries.ListarComPaginacao
{
    public class ListarComPaginacaoPedidoQueryHandler
     : IRequestHandler<ListarComPaginacaoPedidoQuery, RespostaGeral<PaginacaoResultado<PedidoDto>>>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;

        public ListarComPaginacaoPedidoQueryHandler(
            IPedidoRepository pedidoRepository,
            IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }

        public async Task<RespostaGeral<PaginacaoResultado<PedidoDto>>> Handle(
            ListarComPaginacaoPedidoQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var resultado = await _pedidoRepository
                    .ListarPaginadoAsync(request.Pagina, request.Tamanho);

                if (resultado.TotalRegistos == 0)
                {
                    return RespostaGeral<PaginacaoResultado<PedidoDto>>.Falha(
                        "Nenhuma informação encontrada");
                }

                var paginacaoDto = new PaginacaoResultado<PedidoDto>
                {
                    Dados = _mapper.Map<IEnumerable<PedidoDto>>(resultado.Dados),
                    Pagina = resultado.Pagina,
                    TamanhoPagina = resultado.TamanhoPagina,
                    TotalRegistos = resultado.TotalRegistos
                };

                return RespostaGeral<PaginacaoResultado<PedidoDto>>.Ok(
                    paginacaoDto,
                    "Pedidos listados com sucesso");
            }
            catch (Exception ex)
            {
                return RespostaGeral<PaginacaoResultado<PedidoDto>>.Falha(
                    "Erro ao listar pedidos",
                    new List<string> { ex.Message });
            }
        }
    }
}