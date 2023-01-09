using Pokedex.Business.Repositories;
using Pokedex.Business.Services;
using Pokedex.Infra.Repositories;

namespace Pokedex.Api.Configurations
{
    public static class IoC
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPokedexService, PokedexService>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPokemonRepository, PokemonRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;
        }
    }
}