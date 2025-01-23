// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

namespace OnlineShop.Web.Areas.Identity.Pages.Account
{
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;
	using Microsoft.Extensions.Logging;

	using Infrastructure.Identity;
    using Microsoft.AspNetCore.Authorization;

    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LogoutModel(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
			await _signInManager.SignOutAsync();

			HttpContext.Session.Clear();

            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            Response.Cookies.Delete(".AspNetCore.Application.Identity.Application");

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
