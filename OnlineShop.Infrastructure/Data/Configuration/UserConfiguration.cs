namespace OnlineShop.Infrastructure.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

	using Identity;

    using static Domain.Common.EntityValidationConstants.AppUser;

    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder
                .ToTable("Users")
                .ToTable(t => t.HasComment("Table of users"));

            builder
                .HasKey(p => p.Id);

            builder.Property(u => u.FullName)
                    .HasMaxLength(FullNameMaxLength)  
                    .IsRequired()
                    .HasComment("Full name of the user");

        }
    }
}
