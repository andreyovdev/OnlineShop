namespace OnlineShop.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;
    
    using Application.Services.Interfaces;
    using Application.ViewModels.Shop;
	using Microsoft.IdentityModel.Tokens;

	public class ShopController : Controller
    {
        private readonly IProductService productService;
        private const int pageSize = 10; //10 default

        public ShopController(IProductService productService)
        {
            this.productService = productService;
        }

        //OPTIONAL Index (gets all products from database)
        //     [HttpGet]
        //     public async Task<IActionResult> Index(string input, int pg = 1)
        //     {
        //         ViewData["SearchProductInput"] = input;

        //         IEnumerable<AllProductsViewModel> allProduct;

        //         if (string.IsNullOrEmpty(input))
        //         {
        //             allProduct = await this.productService.GetAllProductsAsync();
        //         }
        //         else
        //         {
        //             allProduct = await this.productService.GetAllSearchedProductsAsync(input);
        //         }

        //if (pg < 1) pg = 1;
        //         int prodCount = allProduct.Count();
        //         var pager = new Pager(prodCount, pg, pageSize);
        //         int prodSkip = (pg - 1) * pageSize;
        //         var data = allProduct.Skip(prodSkip).Take(pager.PageSize).ToList();
        //         ViewBag.Pager = pager;

        //         return View(data);
        //     }

        //Gets pageSize amount products from database
        [HttpGet]
        public async Task<IActionResult> Index(string input, int pg = 1)
        {
            ViewData["SearchProductInput"] = input;

            if (pg < 1) pg = 1;

            IEnumerable<AllProductsViewModel> products;
            int prodCount = 0;

            if (string.IsNullOrEmpty(input))
            {
                products = await this.productService.GetAllProductsPagedAsync(pg, pageSize);
                prodCount = await this.productService.GetAllProductsCountAsync();
            }
            else
            {
                products = await this.productService.GetAllSearchedProductsPagedAsync(input, pg, pageSize);
                prodCount = await this.productService.GetAllSearchedProductsCountAsync(input);
            }

            var pager = new Pager(prodCount, pg, pageSize);
            int prodSkip = (pg - 1) * pageSize;
            ViewBag.Pager = pager;

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> AllProductsFiltered([FromBody] FilterOptionsViewModel filters)
        {
			//var products = await this.productService.GetAllProductsFilteredAsync(filters, pg, pageSize);
			//var totalProducts = await this.productService.GetFilteredProductCountAsync(filters);
			
            // Extract currentPage and Categories from the request
			int currentPage = filters.CurrentPage;
            string searchInput = filters.SearchInput;
			List<string> categories = filters.Categories;
			List<string> priceRanges = filters.PriceRanges;
			bool inStock = filters.InStock;
			string sortByPrice = filters.SortByPrice;

			// Define page size (you can adjust this as needed)
			 // Adjust based on how many products you want to display per page
			int skip = (currentPage - 1) * pageSize;

			// Get all products from the product service (without filtering or pagination for now)
			var allProducts = await productService.GetAllProductsAsync();

            if(!searchInput.IsNullOrEmpty())
            {
                allProducts = allProducts.Where(p=>p.Name.Contains(searchInput)).ToList();
            }

			if (priceRanges != null && priceRanges.Any())
			{
				var priceRangeFilters = new List<Func<AllProductsViewModel, bool>>();

				// Add filters based on selected price ranges
				if (priceRanges.Contains("below-200"))
				{
					priceRangeFilters.Add(p => p.Price < 200);
				}
				if (priceRanges.Contains("201-999"))
				{
					priceRangeFilters.Add(p => p.Price >= 201 && p.Price <= 999);
				}
				if (priceRanges.Contains("1000-1999"))
				{
					priceRangeFilters.Add(p => p.Price >= 1000 && p.Price <= 1999);
				}
				if (priceRanges.Contains("above-2000"))
				{
					priceRangeFilters.Add(p => p.Price > 2000);
				}

				allProducts = allProducts.Where(p => priceRangeFilters.Any(filter => filter(p))).ToList();
			}

			if (categories != null && categories.Any())
			{
				allProducts = allProducts.Where(p => categories.Contains(p.Category)).ToList();
			}

            if (inStock)
            {
                allProducts = allProducts.Where(p => p.Quantity > 0).ToList();
            }

            if (!sortByPrice.IsNullOrEmpty())
            { 
                if(sortByPrice == "high-to-low")
                {
                    allProducts = allProducts.OrderByDescending(p => p.Price).ToList();
                } 
                else if (sortByPrice == "low-to-high")
                {
					allProducts = allProducts.OrderBy(p => p.Price).ToList();
				}
			}

				// Apply pagination (skip and take)
				var products = allProducts.Skip(skip).Take(pageSize).ToList();

			// Get the total count of products based on the filters (without pagination)
			int totalProducts = allProducts.Count();

			// Calculate total pages
			int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

			// Construct the response object
			var response = new
			{
				products = products,
				totalPages = totalPages,
				currentPage = currentPage // Ensure currentPage is returned in the response
			};

			// Return the response as JSON
			return Json(response);
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
