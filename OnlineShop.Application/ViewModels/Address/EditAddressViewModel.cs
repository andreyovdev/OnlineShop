using AutoMapper;
using OnlineShop.Application.Mapping;
using OnlineShop.Application.ViewModels.Shop;
using System.ComponentModel.DataAnnotations;
using static OnlineShop.Application.ViewModels.Validation.ViewModelValidationMessages;

namespace OnlineShop.Application.ViewModels.Address
{
	public class EditAddressViewModel : IHaveCustomMappings
	{
		[Required]
		public string Id { get; set; } = null!;

		[Required(ErrorMessage = RequiredMessage)]
		public string Country { get; set; } = null!;

		[Required(ErrorMessage = RequiredMessage)]
		public string City { get; set; } = null!;

		[Required(ErrorMessage = RequiredMessage)]
		public string Street { get; set; } = null!;

		[Required(ErrorMessage = "Phone Number is required.")]
		[RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = InvalidPhoneNumberFormat)]
		public string PhoneNumber { get; set; } = null!;

		public void CreateMappings(IProfileExpression profile)
		{
			profile
				.CreateMap<Domain.Entities.Address, EditAddressViewModel>()
					.ForMember(d => d.Id, x => x.MapFrom(s => s.Id.ToString()));

			profile
				.CreateMap<EditAddressViewModel, Domain.Entities.Address>()
					.ForMember(d => d.UserProfileId, x => x.Ignore())
					.ForMember(d => d.Id, x => x.MapFrom(s => Guid.Parse(s.Id)));
		}

	}
}
