namespace OnlineShop.Infrastructure.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;
	using Identity;

    using static Domain.Common.EntityValidationConstants.UserProfile;

    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder
                .ToTable("UserProfiles")
                .ToTable(t => t.HasComment("Table of user profiles"));

            builder
                .HasKey(p => p.Id);

            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(NameMaxLength)
                .HasComment("Name of the user profile");

            builder.Property(p => p.Email)
              .IsRequired()
              .HasComment("Email of the user profile");

            builder
				.HasOne<AppUser>()
				.WithOne()  
				.HasForeignKey<UserProfile>(up => up.AppUserId);

			builder.HasOne(up => up.Address)
				 .WithOne(a => a.UserProfile)
				.HasForeignKey<Address>(a => a.UserProfileId);



			builder
				.HasMany(up => up.Wishlist)
				.WithOne(w => w.UserProfile)
				.HasForeignKey(w => w.UserProfileId);  

			builder
				.HasMany(up => up.Cart)
				.WithOne(c => c.UserProfile)
				.HasForeignKey(c => c.UserProfileId); 

			builder
				.HasMany(up => up.Purchases)
				.WithOne(p => p.UserProfile)
				.HasForeignKey(p => p.UserProfileId); 

			builder
				.Property(up => up.AppUserId)
				.IsRequired()
				.HasComment("Foreign key to AppUser");

			builder
				.HasIndex(up => up.AppUserId)
				.IsUnique(); 
		}
    }
}
