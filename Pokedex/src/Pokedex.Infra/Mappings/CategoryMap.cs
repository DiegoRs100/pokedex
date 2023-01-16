using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokedex.Business.Entities;

namespace Pokedex.Infra.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("CATEGORY");
            builder.HasKey(pa => pa.Id);

            builder.Property(pa => pa.Id).HasColumnName("CategoryId");

            builder.Property(pa => pa.Name)
                .HasColumnName("Name")
                .IsUnicode()
                .HasMaxLength(50);

            builder.Property(p => p.CreatedAt).HasColumnName("CreatedAt");
            builder.Property(p => p.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}