using GoodHamburger.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodHamburger.Dominio.Interface
{
    public interface IPedidoRepository
    {
        Task CriarAsync(Pedido pedido);
        Task ActualizarAsync(Pedido pedidoAtualizado);
        Task<bool> RemoveAsync(Guid id);
        Task<Pedido?> ConsultarPorIdAsync(Guid id);
        Task<IEnumerable<Pedido>> ListarAsync();
        Task<PaginacaoResultado<Pedido>> ListarPaginadoAsync(int pagina, int tamanhoPagina);
    }
}
