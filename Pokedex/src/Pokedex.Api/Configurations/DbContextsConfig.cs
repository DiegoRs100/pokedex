using Microsoft.EntityFrameworkCore;
using Pokedex.Infra;

namespace Pokedex.Api.Configurations
{
    public static class DbContextsConfig
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionName = configuration["SqlServer:PokedexDb"];
            var connectionString = configuration.GetConnectionString(connectionName!);

            services.AddDbContext<EFDbContext>(options =>
            {
                options.UseSqlServer(connectionString!, options =>
                {
                    options.EnableRetryOnFailure(4, TimeSpan.FromSeconds(20), null);
                });
            });

            services.AddScoped(_ => new DapperDbContext(connectionString!));

            return services;
        }
    }
}