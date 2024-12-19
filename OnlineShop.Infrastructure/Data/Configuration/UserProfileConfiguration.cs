namespace OnlineShop.Infrastructure.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;
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

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength)
                .HasComment("Name of the user");

            builder
                .HasMany(p => p.UserProducts)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId);
        }
    }
}
