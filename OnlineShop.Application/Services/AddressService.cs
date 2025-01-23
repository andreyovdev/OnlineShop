using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Mapping;
using OnlineShop.Application.Services.Interfaces;
using OnlineShop.Application.ViewModels.Address;
using OnlineShop.Application.ViewModels.Shop;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Enums;
using OnlineShop.Infrastructure.Data.Repository.Interfaces;

namespace OnlineShop.Application.Services
{
	public class AddressService : IAddressService
	{
		private readonly IRepository<UserProfile> userProfileRepository;
		private readonly IRepository<Address> addressRepository;

		public AddressService(IRepository<UserProfile> userProfileRepository, IRepository<Address> addressRepository)
		{
			this.userProfileRepository = userProfileRepository;
			this.addressRepository = addressRepository;
		}

		
		public async Task<AddressViewModel> GetAddressAsync(Guid userProfileId)
		{
			var userProfile = await userProfileRepository.GetByIdAsync(userProfileId);
			if (userProfile == null)
			{
				return null;
			}

			var address =  await addressRepository
				.GetAllAttached()
				.Where(a => a.UserProfileId == userProfileId)
				.FirstOrDefaultAsync();

			if (address == null)
			{
				return null;
			}

			return new AddressViewModel
			{
				FullName = userProfile.FullName,
				PhoneNumber = address.PhoneNumber,
				DeliveryAddress = string.Join(", ", address.Street, address.City, address.Country).TrimEnd(','),
			};
		}

		public async Task<bool> UserHasAddress(Guid userProfileId)
		{
			var userProfile = await userProfileRepository.GetByIdAsync(userProfileId);
			if (userProfile == null)
			{
				return false;
			}

			var address = await addressRepository
				.GetAllAttached()
				.Where(a => a.UserProfileId == userProfileId)
				.FirstOrDefaultAsync();

			if (address == null)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> AddAddressAsync(AddAddressViewModel model, Guid userProfileId)
		{
			Address address = new Address();
			AutoMapperConfig.MapperInstance.Map(model, address);

			if (address == null)
			{
				return false;
			}

			address.UserProfileId = userProfileId;

			addressRepository.Add(address);
			await addressRepository.SaveAsync();

			return true;
		}

		public async Task<bool> DeleteAddressAsync(Guid userProfileId)
		{
			var address = await this.addressRepository
				.GetAllAttached()
				.Where(a => a.UserProfileId == userProfileId)
				.FirstOrDefaultAsync();

			if (address == null)
			{
				return false;
			}

			addressRepository.RemoveByEntity(address);
			await addressRepository.SaveAsync();

			return true;
		}

		public async Task<bool> EditAddressAsync(EditAddressViewModel model, Guid userProfileId)
		{
			Address address = new Address();
			AutoMapperConfig.MapperInstance.Map(model, address);

			address.UserProfileId = userProfileId;

			if (address == null)
			{
				return false;
			}

			addressRepository.Update(address);
			await addressRepository.SaveAsync();

			return true;
		}

		public async Task<EditAddressViewModel> GetEditAddressAsync(Guid userProfileId)
		{
			var userProfile = await userProfileRepository.GetByIdAsync(userProfileId);
			if (userProfile == null)
			{
				return null;
			}

			var address = await addressRepository
				.GetAllAttached()
				.Where(a => a.UserProfileId == userProfileId)
				.To<EditAddressViewModel>()
				.FirstOrDefaultAsync();

			

			if (address == null)
			{
				return null;
			}

			return address;
		}
	}
}
