namespace OnlineShop.Infrastructure.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;

    using static Domain.Common.EntityValidationConstants.Product;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .ToTable("Products")
                .ToTable(t => t.HasComment("Table of products"));

            builder
                .HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength)
                .HasComment("Name of the product");

            builder.Property(p => p.Category)
                .IsRequired();

            builder.Property(p => p.ImgUrl)
                .IsRequired()
                .HasComment("URL of the product image");

            builder.Property(p => p.Price)
                .HasDefaultValue(0)
                .HasComment("Price of the product");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength)
                .HasComment("Description of the product");

            builder.Property(p => p.Quantity)
                .HasDefaultValue(0)
                .HasComment("Available quantity of the product");

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false)
                .HasComment("Indicates if the product is deleted");
        }
    }
}
