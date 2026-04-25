using GoodHamburger.Aplicacao.DTOs;
using GoodHamburger.Dominio.Entidade;
using MediatR;

namespace GoodHamburger.Aplicacao.UseCases.Pedido.Queries.ListarComPaginacao
{
    public class ListarComPaginacaoPedidoQuery : IRequest<RespostaGeral<PaginacaoResultado<PedidoDto>>>
    {
        public int Pagina { get; set; } = 1;
        public int Tamanho { get; set; } = 10;

        public ListarComPaginacaoPedidoQuery(int pagina, int tamanho)
        {
            Pagina=pagina;
            Tamanho=tamanho;
        }
    }
}