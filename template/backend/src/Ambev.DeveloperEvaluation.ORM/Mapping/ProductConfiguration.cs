using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Code).HasMaxLength(10);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.StockQuantity).IsRequired();
        builder.Property(p => p.SKU).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Active).IsRequired().HasDefaultValue(true);
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt);

        builder.HasIndex(p => p.Code).IsUnique();
    }
} 