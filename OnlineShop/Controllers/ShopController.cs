using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Web.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //authorize as admin
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {

            return View();
        }
    }
}
