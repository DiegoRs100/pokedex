using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokedex.Business.Entities;

namespace Pokedex.Infra.Mappings
{
    public class PokemonAbilityMap : IEntityTypeConfiguration<PokemonAbility>
    {
        public void Configure(EntityTypeBuilder<PokemonAbility> builder)
        {
            builder.ToTable("POKEMON_ABILITIES");
            builder.HasKey(pa => pa.Id);

            builder.Property(pa => pa.Id).HasColumnName("PokemonAbilityId");
            builder.Property(pa => pa.PokemonId).HasColumnName("PokemonId");

            builder.Property(pa => pa.Name)
                .HasColumnName("Name")
                .IsUnicode()
                .HasMaxLength(50);
        }
    }
}