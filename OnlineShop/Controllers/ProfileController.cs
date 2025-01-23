namespace OnlineShop.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OnlineShop.Application.Extensions;
    using OnlineShop.Application.Services;
    using OnlineShop.Application.Services.Interfaces;
    using OnlineShop.Application.ViewModels.Profile;
    using OnlineShop.Domain.Entities;

    public class ProfileController : Controller
    {
        private readonly IUserProfileService userProfileService;

        public ProfileController(IUserProfileService userProfileService)
        {
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

            UserProfileViewModel userProfile = await userProfileService.GetUserProfileById(userProfileGuid);

            return View(userProfile);
        }
	}
}
