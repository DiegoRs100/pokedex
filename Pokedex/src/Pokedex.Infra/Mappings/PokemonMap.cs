using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Pokedex.Business.Entities;

namespace Pokedex.Infra.Mappings
{
    public class PokemonMap : IEntityTypeConfiguration<Pokemon>
    {
        public void Configure(EntityTypeBuilder<Pokemon> builder)
        {
            builder.ToTable("POKEMONS");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("IdPokemon");
            builder.Property(p => p.Gender).HasColumnName("Gender");

            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .IsUnicode()
                .HasMaxLength(50);

            builder.HasMany(p => p.Abilities)
                .WithOne()
                .HasForeignKey(p => p.PokemonId);
        }
    }
}