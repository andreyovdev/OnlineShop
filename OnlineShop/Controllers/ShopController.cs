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

        //authorize as admin
        [HttpPost]
        public async Task<IActionResult> AddNewProduct(AddNewProductViewModel model)
        {
            ViewData["HideLayoutParts"] = true;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.productService.AddNewProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

        //authorize as admin
        [HttpGet]
        public async Task<IActionResult> EditProduct(string id)
        {
            ViewData["HideLayoutParts"] = true;

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

        //authorize as admin
        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductViewModel model)
        {
            ViewData["HideLayoutParts"] = true;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await productService.EditProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

        //authorize as admin
        [HttpGet]
        public async Task<IActionResult> RemoveProduct(string id)
        {
            ViewData["HideLayoutParts"] = true;

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
        
        //authorize as admin
        [HttpPost]
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
