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

            builder.Property(p => p.Id).HasColumnName("PokemonId");
            builder.Property(p => p.Gender).HasColumnName("Gender");

            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(p => p.CategoryId).HasColumnName("CategoryId");
            builder.Property(p => p.Hp).HasColumnName("Hp");
            builder.Property(p => p.Attack).HasColumnName("Attack");
            builder.Property(p => p.Defense).HasColumnName("Defense");
            builder.Property(p => p.Speed).HasColumnName("Speed");

            builder.Property(p => p.CreatedAt).HasColumnName("CreatedAt");
            builder.Property(p => p.UpdatedAt).HasColumnName("UpdatedAt");

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);
        }
    }
}