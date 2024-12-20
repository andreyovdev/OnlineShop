namespace OnlineShop.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;
    
    using Application.Services.Interfaces;
    using Application.ViewModels.Shop;

	public class ShopController : Controller
	{
        private readonly IProductService productService;

        public ShopController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string input, int pg = 1)
        {
            ViewData["SearchProductInput"] = input;

            IEnumerable<AllProductsViewModel> allProduct;

            if (string.IsNullOrEmpty(input))
            {
                allProduct = await this.productService.GetAllProductsAsync();
            }
            else
            {
                allProduct = await this.productService.SearchProductsAsync(input);
            }

            const int pageSize = 10; //1 for testing
            if (pg < 1) pg = 1;
            int prodCount = allProduct.Count();
            var pager = new Pager(prodCount, pg, pageSize);
            int prodSkip = (pg - 1) * pageSize;
            var data = allProduct.Skip(prodSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            return View(data);
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

        //move
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
