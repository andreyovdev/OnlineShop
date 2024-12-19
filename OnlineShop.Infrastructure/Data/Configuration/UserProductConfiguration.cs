namespace OnlineShop.Infrastructure.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;

    public class UserProductConfiguration : IEntityTypeConfiguration<UserProduct>
    {
        public void Configure(EntityTypeBuilder<UserProduct> builder)
        {
            builder
                .ToTable("UserProducts")
                .ToTable(t => t.HasComment("Table of products and users"));

            builder
                .HasKey(up => new { up.UserId, up.ProductId });

            builder
                .HasOne(up => up.User) 
                .WithMany(u => u.UserProducts)
                .HasForeignKey(up => up.UserId);

            builder
                .HasOne(up => up.Product)
                .WithMany(p => p.ProductUsers)
                .HasForeignKey(up => up.ProductId);

            builder
                .Property(up => up.UserId)
                .IsRequired()
                .HasComment("Id of the user");

            builder
                .Property(up => up.ProductId)
                .IsRequired()
                .HasComment("Id of the product");
        }
    }
}
