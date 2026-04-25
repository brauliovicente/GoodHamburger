using GoodHamburger.Dominio.Entidade;

namespace GoodHamburger.Dominio.Interface
{
    public interface IItemRepository
    {
        Task<Item> CriarAsync(Item item);
        Task<Item> ActualizarAsync(Item item);
        Task RemoverAsync(Guid id);
        Task<Item?> ConsultarPorIdAsync(Guid id);
        Task<List<Item>?> ConsultarPorIdsAsync(List<Guid> listaIDs);
        Task<Item?> ConsultarPorNomeAsync(string nome);
        Task<IEnumerable<Item>> ListarAsync();
       
    }
}