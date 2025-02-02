namespace OnlineShop.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using Application.Extensions;
	using Application.Services.Interfaces;

	public class HeaderController : Controller
	{
		private readonly IUserProfileService userProfileService;
		private readonly IWishlistService wishlistService;
		private readonly ICartService cartService;

		public HeaderController(IUserProfileService userProfileService, IWishlistService wishlistService, ICartService cartService)
		{
			this.userProfileService = userProfileService;
			this.wishlistService = wishlistService;
			this.cartService = cartService;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetProductsInWishlistCount()
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			int productsInWishlist = await wishlistService.GetProductsInWishlistCountAsync(userProfileGuid);

			return Json(new { productsInWishlist});
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetProductsInCartCount()
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			int productsInCart = await cartService.GetProductsInCartCountAsync(userProfileGuid);

			return Json(new { productsInCart });
		}
	}
}
