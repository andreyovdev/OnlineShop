namespace OnlineShop.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;

    using Domain.Entities;
    using Configuration;
    using Identity;
	using System.Diagnostics;

    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
		}

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
		public DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public DbSet<Wishlist> Wishlists { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<Purchase> Purchases { get; set; } = null!;
		public override DbSet<AppUser> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
        }
    }
}
