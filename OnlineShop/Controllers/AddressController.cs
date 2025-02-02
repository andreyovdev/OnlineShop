namespace OnlineShop.Web.Controllers
{
	using System.Diagnostics;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

	using Application.Extensions;
	using Application.Services.Interfaces;
	using Application.ViewModels.Address;

	public class AddressController : Controller
    {
		private readonly IUserProfileService userProfileService;
		private readonly IAddressService addressService;

		public AddressController(IAddressService addressService, IUserProfileService userProfileService)
		{
			this.addressService = addressService;
			this.userProfileService = userProfileService;
		}

		[HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			var model = await this.addressService.GetAddressAsync(userProfileGuid);

		
			return View(model);
        }

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> AddAddress()
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}


			bool addressExists = await this.addressService.UserHasAddress(userProfileGuid);

			if (addressExists)
			{
				return RedirectToAction("Index");
			}

			return View();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddAddress(AddAddressViewModel model)
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			if (!ModelState.IsValid)
			{
				foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
				{
					Debug.WriteLine($"Error: {error.ErrorMessage}");
				}
				return View(model);
			}

			bool result = await this.addressService.AddAddressAsync(model, userProfileGuid);

			if (!result)
			{
				return View();
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteAddress()
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return RedirectToAction("Index", "Home");
			}

			var result = await this.addressService.DeleteAddressAsync(userProfileGuid);

			if (result)
			{
				return RedirectToAction("Index", "Address"); 
			}

			return RedirectToAction("Index", "Address");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> EditAddress()
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			EditAddressViewModel formModel = await addressService
				.GetEditAddressAsync(userProfileGuid);

			return View(formModel);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> EditAddress(EditAddressViewModel model)
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			bool result = await addressService.EditAddressAsync(model, userProfileGuid);

			if (!result)
			{
				return View();
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
