namespace OnlineShop.Infrastructure.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;

	using static Domain.Common.EntityValidationConstants.Address;

	public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .ToTable("Addresses")
                .ToTable(t => t.HasComment("Table of products in users addresses"));

			builder.HasKey(a => a.Id);

			builder.HasOne(a => a.UserProfile)
				.WithOne(up => up.Address)  
				.HasForeignKey<Address>(a => a.UserProfileId);  

			builder.Property(a => a.Country)
				.IsRequired() 
				.HasMaxLength(CountryMaxLength) 
				.HasComment("Country of the user's address");

			builder.Property(a => a.City)
				.IsRequired()
				.HasMaxLength(CityMaxLength)
				.HasComment("City of the user's address");

			builder.Property(a => a.Street)
				.IsRequired()
				.HasMaxLength(StreetMaxLength)
				.HasComment("Street of the user's address");

			builder.Property(a => a.PhoneNumber)
				.IsRequired()  
				.HasMaxLength(PhoneNumberMaxLength)  
				.HasComment("User's phone number for contact")
				.HasAnnotation("PhoneRegex", @"^\+?\d{7,15}$"); 

		}
	}
}
