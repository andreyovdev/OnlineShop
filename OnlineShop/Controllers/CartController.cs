namespace OnlineShop.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.IdentityModel.Tokens;
    
    using Application.Services.Interfaces;
    using Application.ViewModels.Shop;
	using OnlineShop.Application.Extensions;
	using Microsoft.AspNetCore.Cors.Infrastructure;
	using OnlineShop.Application.Services;
	using OnlineShop.Application.ViewModels.Cart;

	public class CartController : Controller
    {
		private readonly IAddressService addressService;
		private readonly ICartService cartService;
		private readonly IUserProfileService userProfileService;

		public CartController(ICartService cartService, IUserProfileService userProfileService, IAddressService addressService)
		{
			this.cartService = cartService;
			this.userProfileService = userProfileService;
			this.addressService = addressService;
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

			if (model == null)
			{
				return NotFound();
			}

			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AllProductsInCart()
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			var prodcutsInCart = await this.cartService.GetProductsInCart(userProfileGuid);

			var response = new
			{
				products = prodcutsInCart,
			};

			return Json(response);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> UpdateCart([FromBody] UpdateCartViewModel model)
		{
			Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

			if (userProfileGuid == Guid.Empty)
			{
				return View();
			}

			Guid productGuid = Guid.NewGuid();

			if (!this.IsGuidValid(model.ProductId, ref productGuid))
			{
				return View();
			}

			int result = await this.cartService.UpdateCartProductCount(userProfileGuid, productGuid, model.Quantity);
			
			if(result == -1)
			{
				return View();
			}

			return Ok(result);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddToCart([FromBody] string productId)
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

			bool result = await this.cartService.AddProductToCart(userProfileGuid, productGuid);

			return Ok(result);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> RemoveFromCart([FromBody] string productId)
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

			bool result = await this.cartService.RemoveProductFromCart(userProfileGuid, productGuid);

			if (!result)
			{
				return View();
			}

			return Ok();
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> IsProductInCart([FromQuery] string productId)
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

			bool result = await this.cartService.IsProductInCart(userProfileGuid, productGuid);

			return Ok(result);
		}

    }
}
