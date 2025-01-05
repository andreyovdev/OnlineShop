namespace OnlineShop.Infrastructure.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;

    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
			builder
			   .ToTable("Purchases")
			   .ToTable(t => t.HasComment("Table of products purchased by users"));

			builder
				.HasKey(c => new { c.UserProfileId, c.ProductId });

			builder
			 .HasOne(p => p.UserProfile)
			 .WithMany(u => u.Purchases) 
			 .HasForeignKey(p => p.UserProfileId); 

			builder
				.HasOne(p => p.Product)
				.WithMany(pr => pr.PurchasedByUsers) 
				.HasForeignKey(p => p.ProductId); 

			builder
				.Property(p => p.Quantity)
				.IsRequired()
				.HasComment("Quantity of the product purchased");

			builder
				.Property(p => p.DatePurchased)
				.IsRequired()
				.HasDefaultValueSql("CURRENT_TIMESTAMP")  
				.HasComment("Date when the product was purchased");

			builder
				.Property(p => p.UserProfileId)
				.IsRequired()
				.HasComment("Id of the user");

			builder
				.Property(p => p.ProductId)
				.IsRequired()
				.HasComment("Id of the product");
		}
    }
}
