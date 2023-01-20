using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pokedex.Business.Entities;

namespace Pokedex.Infra
{
    public class EFDbContext : DbContext
    {
        private readonly IHostingEnvironment _enviroment;

        public DbSet<Pokemon> Pokemons { get; set; }

        public EFDbContext(DbContextOptions<EFDbContext> options, IHostingEnvironment enviroment) : base(options)
        {
            _enviroment = enviroment;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var logConfig = new[]
            {
                RelationalEventId.ConnectionOpened,
                RelationalEventId.ConnectionClosed,
                RelationalEventId.TransactionStarted,
                RelationalEventId.TransactionCommitted,
                RelationalEventId.TransactionRolledBack,
                RelationalEventId.CommandExecuted
            };

            optionsBuilder
                .LogTo(Console.WriteLine, logConfig)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);

            if (!_enviroment.IsProduction())
                optionsBuilder.EnableDetailedErrors().EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }
    }
}