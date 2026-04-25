using GoodHamburger.Dominio.Interface;
using GoodHamburger.Infraestrutura.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.Infraestrutura.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Repositórios
            services.AddMemoryCache();

            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();

            return services;
        }
    }
}