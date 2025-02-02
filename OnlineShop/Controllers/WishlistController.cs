namespace OnlineShop.Web.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using Application.Services.Interfaces;

	using static Application.Extensions.ControllerExtensions;
	
	public class WishlistController : Controller
	{
		private readonly IWishlistService wishlistService;
		private readonly IUserProfileService userProfileService;

		public WishlistController(IWishlistService wishlistService, IUserProfileService userProfileService)
		{
			this.wishlistService = wishlistService;
			this.userProfileService = userProfileService;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Index()
		{
			return View();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AllProductsInWishlist()
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			var productsInWishlist = await this.wishlistService.GetProductsInWishlist(userProfileGuid);

			var response = new
			{
				products = productsInWishlist,
			};

			return Json(response);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddToWishlist([FromBody] string productId)
		{
			Guid productGuid = Guid.NewGuid();

			if (!this.IsGuidValid(productId, ref productGuid))
			{
				return View();
			}

			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			bool result = await this.wishlistService.AddProductToWishlist(userProfileGuid, productGuid);

			if (!result)
			{
				return View();
			}

			return Ok();
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> RemoveFromWishlist([FromBody] string productId)
		{
			Guid productGuid = Guid.NewGuid();

			if (!this.IsGuidValid(productId, ref productGuid))
			{
				return View();
			}

			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			bool result = await this.wishlistService.RemoveProductFromWishlist(userProfileGuid, productGuid);

			if (!result)
			{
				return View();
			}

			return Ok();
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> IsProductInWishlist([FromQuery] string productId)
		{
			Guid productGuid = Guid.NewGuid();

			if (!this.IsGuidValid(productId, ref productGuid))
			{
				return View();
			}

			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return Ok(false);
			}

			bool result = await this.wishlistService.IsProductInWishlist(userProfileGuid, productGuid);

			return Ok(result);
		}
	}
}
