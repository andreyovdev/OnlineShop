﻿namespace OnlineShop.Web.Controllers
{
	using Application.Services.Interfaces;
	using Application.ViewModels.Shop;
	using DotNetEd.CoreAdmin.Controllers;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Components.RenderTree;
	using Microsoft.AspNetCore.Mvc;

	using static Application.Extensions.ControllerExtensions;

	public class ShopController : Controller
    {
        private const int pageSize = 10; //Products displayed per page. 10 is default

		private readonly IProductService productService;
		private readonly IWishlistService wishlistService;
		private readonly IUserProfileService userProfileService;

		public ShopController(IProductService productService, IWishlistService wishlistService, IUserProfileService userProfileService)
        {
            this.productService = productService;
            this.wishlistService = wishlistService;
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

        [HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddNewProduct()
        {
			HttpContext.Session.SetString("PreviousPage", Request.Headers["Referer"].ToString());

			return View();
        }

        [HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddNewProduct(AddNewProductViewModel model)
        {
			string previousPage = HttpContext.Session.GetString("PreviousPage");

			if (!ModelState.IsValid)
            {
                return View(model);
            }

			bool result = await this.productService.AddNewProductAsync(model);

            if (!result)
            {
                return View();
            }

			if (previousPage.Contains("CoreAdmin"))
			{
				return Redirect(previousPage);
			}
            else
            {
				return RedirectToAction(nameof(Index));
			}
            
        }

        [HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditProduct(string id)
        {
			HttpContext.Session.SetString("PreviousPage", Request.Headers["Referer"].ToString());

			Guid productGuid = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref productGuid);
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
			string previousPage = HttpContext.Session.GetString("PreviousPage");

			if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await productService.EditProductAsync(model);

			if (!result)
			{
				return View();
			}

			if (previousPage.Contains("CoreAdmin"))
			{
				return Redirect(previousPage);
			}
			else
			{
				return RedirectToAction(nameof(Index));
			}
        }

		[HttpGet]
		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProduct(string id)
        {
			HttpContext.Session.SetString("PreviousPage", Request.Headers["Referer"].ToString());

			Guid productGuid = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref productGuid);
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
			string previousPage = HttpContext.Session.GetString("PreviousPage");

			Guid productGuid = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref productGuid);

            bool result = await productService.RemoveProductAsync(productGuid);

			if (!result)
			{
				return View();
			}

			if (previousPage.Contains("CoreAdmin"))
			{
				return Redirect(previousPage);
			}
			else
			{
				return RedirectToAction(nameof(Index));
			}
		}

        [HttpGet]
        public async Task<IActionResult> ProductDetails(string id)
        {
            Guid productGuid = Guid.Empty;
            bool isIdValid = this.IsGuidValid(id, ref productGuid);
            if (!isIdValid)
            {
                return RedirectToAction(nameof(Index));
            }

            ProductDetailsViewModel product = 
             await this.productService.GetProductDetailsAsync(productGuid);

            return View(product);
        }

	}
}
