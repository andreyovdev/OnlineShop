namespace OnlineShop.Application.ViewModels.Address
{
	using System.ComponentModel.DataAnnotations;

	using Mapping;

	using static OnlineShop.Application.ViewModels.Validation.ViewModelValidationMessages;

	public class AddAddressViewModel : IMapTo<Domain.Entities.Address>
	{
		[Required(ErrorMessage = RequiredMessage)]
		public string Country { get; set; } = null!;

		[Required(ErrorMessage = RequiredMessage)]
		public string City { get; set; } = null!;

		[Required(ErrorMessage = RequiredMessage)]
		public string Street { get; set; } = null!;

		[Required(ErrorMessage = "Phone Number is required.")]
		[RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = InvalidPhoneNumberFormat)]
		public string PhoneNumber { get; set; } = null!;

	}
}
