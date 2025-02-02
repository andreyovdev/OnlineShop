namespace OnlineShop.Application.Services.Interfaces
{
	using ViewModels.Address;

	public interface IAddressService
	{
		Task<AddressViewModel> GetAddressAsync(Guid userProfileId);
		Task<bool> UserHasAddress(Guid userProfileId);
		Task<bool> AddAddressAsync(AddAddressViewModel model, Guid userProfileId);
		Task<bool> EditAddressAsync(EditAddressViewModel model, Guid userProfileId);
		Task<EditAddressViewModel> GetEditAddressAsync(Guid userProfileId);
		Task<bool> DeleteAddressAsync(Guid userProfileId);
	}
}
