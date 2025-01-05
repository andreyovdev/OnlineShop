namespace OnlineShop.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.IdentityModel.Tokens;
    
    using Application.Services.Interfaces;
    using Application.ViewModels.Shop;
    using static NuGet.Packaging.PackagingConstants;

	public class ShopController : Controller
    {
        private const int pageSize = 10; //Products displayed per page. 10 is default

		private readonly IProductService productService;
		private readonly IUserProfileService userProfileService;

		public ShopController(IProductService productService, IUserProfileService userProfileService)
        {
            this.productService = productService;
            this.userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> AllProductsFiltered([FromBody] FilterOptionsViewModel filters)
		{
			int currentPage = filters.CurrentPage;
			int chunkIndex = (currentPage-1) * pageSize; 
			int chunkSize = pageSize;   

			int skip = chunkIndex * chunkSize;

			var filteredProducts = await this.productService.GetProductsChunkFilteredAsync(filters, chunkIndex, chunkSize);

			int totalFilteredProducts = await this.productService.GetAllFilteredProductCountAsync(filters);

			int totalPages = (int)Math.Ceiling((double)totalFilteredProducts / chunkSize);

			var response = new
			{
				products = filteredProducts,
				totalPages = totalPages,
				currentPage = currentPage
			};

			return Json(response);
		}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToWishlist([FromBody] string id)
        {
            //await this.userProfileService.AddProductToWishlist(id);

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] string id)
        {
            //await this.userService.AddProductToCart(id);

            return Ok();
        }

        [HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddNewProduct()
        {
			return View();
        }

        [HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddNewProduct(AddNewProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.productService.AddNewProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditProduct(string id)
        {

            Guid productGuid = Guid.Empty;
            bool isIdValid = IsGuidValid(id, ref productGuid);
            if (!isIdValid)
            {
                return RedirectToAction(nameof(Index));
            }

            EditProductViewModel formModel = await productService
                .GetEditProductByIdAsync(productGuid);

            return View(formModel);
        }

		[HttpPost]
		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(EditProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await productService.EditProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

		[HttpGet]
		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProduct(string id)
        {
            Guid productGuid = Guid.Empty;
            bool isIdValid = IsGuidValid(id, ref productGuid);
            if (!isIdValid)
            {
                return RedirectToAction(nameof(Index));
            }

            RemoveProductViewModel formModel = await productService
                .GetRemoveProductByIdAsync(productGuid);

            return View(formModel);
        }

		[HttpPost]
		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProduct(string id, RemoveProductViewModel model)
        {
            Guid productGuid = Guid.Empty;
            bool isIdValid = IsGuidValid(id, ref productGuid);

            await productService.RemoveProductAsync(productGuid);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(string id)
        {
            Guid productGuid = Guid.Empty;
            bool isIdValid = IsGuidValid(id, ref productGuid);
            if (!isIdValid)
            {
                return RedirectToAction(nameof(Index));
            }

            ProductDetailsViewModel product = 
             await this.productService.GetProductDetailsAsync(productGuid);

            return View(product);
        }

        private bool IsGuidValid(string? id, ref Guid productId)
        {
            //Invalid parameter in URL
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            //Invalid id in url
            bool isGuidValid = Guid.TryParse(id, out productId);
            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }
    }
}
