using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GoodHamburger.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // MediatR
            services.AddMediatR(assembly);

            // FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            // Mapster
            var config = TypeAdapterConfig.GlobalSettings;

            // Registra todos os mappings do assembly (IRegister)
            config.Scan(assembly);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}