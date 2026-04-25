using GoodHamburger.Dominio.Entidade;
using GoodHamburger.Dominio.Interface;
using Microsoft.Extensions.Caching.Memory;

public class PedidoRepository : IPedidoRepository
{
    private readonly IMemoryCache _cache;
    private static readonly object _lock = new();

    private const string CACHE_KEY = "PEDIDOS";

    public PedidoRepository(IMemoryCache cache)
    {
        _cache = cache;
    }

    private List<Pedido> ObterLista()
    {
        return _cache.GetOrCreate(CACHE_KEY, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromHours(1);
            return new List<Pedido>();
        })!;
    }

    public Task CriarAsync(Pedido pedido)
    {
        lock (_lock)
        {
            var lista = ObterLista();
            lista.Add(pedido);
        }

        return Task.CompletedTask;
    }

    public Task ActualizarAsync(Pedido pedidoAtualizado)
    {
        lock (_lock)
        {
            var lista = ObterLista();

            var existente = lista.FirstOrDefault(p => p.Id == pedidoAtualizado.Id);

            if (existente is null)
                return Task.CompletedTask;

            // NÃO substitui referência → mantém integridade
            lista.Remove(existente);
            lista.Add(pedidoAtualizado);
        }

        return Task.CompletedTask;
    }

    public Task<bool> RemoveAsync(Guid id)
    {
        lock (_lock)
        {
            var lista = ObterLista();

            var pedido = lista.FirstOrDefault(p => p.Id == id);

            if (pedido is null)
                return Task.FromResult(false);

            lista.Remove(pedido);
            return Task.FromResult(true);
        }
    }

    public Task<Pedido?> ConsultarPorIdAsync(Guid id)
    {
        var lista = ObterLista();

        var pedido = lista.FirstOrDefault(p => p.Id == id);

        return Task.FromResult(pedido);
    }

    public Task<PaginacaoResultado<Pedido>> ListarPaginadoAsync(int pagina, int tamanhoPagina)
    {
        if (pagina <= 0)
            throw new ArgumentException("Página deve ser maior que 0");

        if (tamanhoPagina <= 0)
            throw new ArgumentException("Tamanho da página deve ser maior que 0");

        var lista = ObterLista();

        var total = lista.Count;

        var dados = lista
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToList();

        return Task.FromResult(new PaginacaoResultado<Pedido>
        {
            Dados = dados,
            TotalRegistos = total,
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina
        });
    }

    public Task<IEnumerable<Pedido>> ListarAsync()
    {
        var lista = ObterLista();

        // Retorna cópia
        return Task.FromResult(lista.ToList().AsEnumerable());
    }
}