using OnlineShop.Application.ViewModels.Address;
using OnlineShop.Application.ViewModels.Shop;

namespace OnlineShop.Application.Services.Interfaces
{
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
