namespace OnlineShop.Infrastructure.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;

    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder
                .ToTable("Carts")
                .ToTable(t => t.HasComment("Table of products in users carts"));

			builder
				.HasKey(c => new { c.UserProfileId, c.ProductId });

			builder
				.HasOne(c => c.UserProfile)
				.WithMany(u => u.Cart) 
				.HasForeignKey(c => c.UserProfileId); 

			builder
				.HasOne(c => c.Product)
				.WithMany(p => p.InCartByUsers) 
				.HasForeignKey(c => c.ProductId); 

			builder
				.Property(c => c.Quantity)
				.IsRequired()
				.HasComment("Quantity of the product in the cart");

			builder
				.Property(c => c.UserProfileId)
				.IsRequired()
				.HasComment("Id of the user profile");

			builder
				.Property(c => c.ProductId)
				.IsRequired()
				.HasComment("Id of the product");
		}
    }
}
