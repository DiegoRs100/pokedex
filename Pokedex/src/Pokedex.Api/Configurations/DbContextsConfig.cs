using Microsoft.EntityFrameworkCore;
using Pokedex.Infra;

namespace Pokedex.Api.Configurations
{
    public static class DbContextsConfig
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionName = configuration["SqlServer:PokedexDb"];

            services.AddDbContext<EFDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(connectionName!), options =>
                {
                    options.EnableRetryOnFailure(4, TimeSpan.FromSeconds(20), null);
                });
            });

            return services;
        }
    }
}