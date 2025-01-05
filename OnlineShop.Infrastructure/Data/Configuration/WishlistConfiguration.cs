namespace OnlineShop.Infrastructure.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;

    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder
                .ToTable("Wishlists")
                .ToTable(t => t.HasComment("Table of products wishlisted users"));

			builder
				.HasKey(w => new { w.UserProfileId, w.ProductId }); 

			builder
				.HasOne(w => w.UserProfile) 
				.WithMany(u => u.Wishlist)  
				.HasForeignKey(w => w.UserProfileId); 

			builder
				.HasOne(w => w.Product)
				.WithMany(p => p.WishlistedByUsers) 
				.HasForeignKey(w => w.ProductId); 

			builder
				.Property(w => w.UserProfileId)
				.IsRequired()
				.HasComment("Id of the user profile");

			builder
				.Property(w => w.ProductId)
				.IsRequired()
				.HasComment("Id of the product");
		}
    }
}
