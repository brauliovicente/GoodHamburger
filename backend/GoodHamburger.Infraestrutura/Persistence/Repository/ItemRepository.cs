using GoodHamburger.Dominio.Interface;
using GoodHamburger.Dominio.Entidade;
using GoodHamburger.Dominio.Enum;

namespace GoodHamburger.Infraestrutura.Persistence.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly List<Item> _items;

        public ItemRepository()
        {
            // Inicializa a lista com alguns itens padrão
            _items = new List<Item>
            {
                new Item(Guid.Parse("11111111-1111-1111-1111-111111111111"), "X Burger", 5.00m, Tipo.Sanduiche),
                new Item(Guid.Parse("22222222-2222-2222-2222-222222222222"), "X Egg", 4.50m, Tipo.Sanduiche),
                new Item(Guid.Parse("33333333-3333-3333-3333-333333333333"), "X Bacon", 7.00m, Tipo.Sanduiche),
                new Item(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Batata frita", 2.00m, Tipo.Batata),
                new Item(Guid.Parse("55555555-5555-5555-5555-555555555555"), "Refrigerante", 2.50m, Tipo.Refrigerante)
            };
        }

        public async Task<Item> CriarAsync(Item item)
        {
            _items.Add(item);
            return await Task.FromResult(item);
        }

        public async Task<Item> ActualizarAsync(Item item)
        {
            var index = _items.IndexOf(item);
            _items[index] = item;

            return await Task.FromResult(item);
        }

        public async Task RemoverAsync(Guid id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);

            _items.Remove(item);

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Item>> ListarAsync()
        {
            return await Task.FromResult(_items);
        }

        public async Task<Item?> ConsultarPorIdAsync(Guid id)
        {
            return await Task.FromResult(_items.FirstOrDefault(i => i.Id == id));
        }

        public async Task<List<Item>?> ConsultarPorIdsAsync(List<Guid> listaIDs)
        {
            if (listaIDs == null || !listaIDs.Any())
                return new List<Item>();

            var items = _items
                .Where(i => listaIDs.Contains(i.Id))
                .ToList();

            return await Task.FromResult(items);
        }

        public async Task<Item?> ConsultarPorNomeAsync(string nome)
        {
            return await Task.FromResult(_items.FirstOrDefault(i => i.Nome == nome));
        }
    }
}