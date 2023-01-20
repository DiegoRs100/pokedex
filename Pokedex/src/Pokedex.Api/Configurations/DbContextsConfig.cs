using Microsoft.EntityFrameworkCore;
using Pokedex.Infra;
using Pokedex.Infra.Interceptors;

namespace Pokedex.Api.Configurations
{
    public static class DbContextsConfig
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["SqlServer:PokedexDb"];

            services.AddDbContext<EFDbContext>(options =>
            {
                options.UseSqlServer(connectionString!, options =>
                {
                    options.EnableRetryOnFailure(4, TimeSpan.FromSeconds(20), null);
                });

                options.AddInterceptors(new EfMetadataInterceptor());
            });

            services.AddScoped(_ => new DapperDbContext(connectionString!));

            return services;
        }
    }
}