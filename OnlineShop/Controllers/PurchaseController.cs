namespace OnlineShop.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineShop.Application.Extensions;
    using OnlineShop.Application.Services;
    using OnlineShop.Application.Services.Interfaces;

	public class PurchaseController : Controller
    {
        private readonly IPurchaseService purchaseService;
        private readonly IUserProfileService userProfileService;

        public PurchaseController(IPurchaseService purchaseService, IUserProfileService userProfileService)
        {
            this.purchaseService = purchaseService;
            this.userProfileService = userProfileService;
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

            var purchases = await this.purchaseService.GetAllPurchasesAsync(userProfileGuid);

            return View(purchases);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreatePurchase()
        {
            Guid userProfileGuid = await this.GetUserProfileGuid(userProfileService);

            if (userProfileGuid == Guid.Empty)
            {
                return View();
            }

            var result = await this.purchaseService.CreatePurchase(userProfileGuid);

            if (result)
            {
                return View("ThankYou");
            }

            return View();
        }
    }
}
