using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Web.Controllers
{
	public class WishlistController : Controller
	{
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}
