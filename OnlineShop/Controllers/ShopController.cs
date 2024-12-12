namespace OnlineShop.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
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
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllProductsViewModel> allProduct =
             await this.productService.GetAllProductsAsync();

            return View(allProduct);
        }

        //authorize as admin
        [HttpGet]
        public async Task<IActionResult> AddNewProduct()
        {
			ViewData["HideLayoutParts"] = true;

			return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProduct(AddNewProductViewModel model)
        {
            ViewData["HideLayoutParts"] = true;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await this.productService.AddNewProductAsync(model);

            if (result == false)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
