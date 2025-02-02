namespace OnlineShop.Application.Extensions
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Http;

	using Services.Interfaces;
	using Infrastructure.Extensions;

	public static class ControllerExtensions
	{
		public static async Task<Guid> GetUserProfileGuid(this Controller controller, IUserProfileService userProfileService)
		{
			string? userId = controller.User.GetUserId();

			if (userId == null)
			{
				return Guid.Empty;
			}

			Guid userGuid = Guid.Parse(userId!);

			var session = controller.HttpContext.Session;

			if (string.IsNullOrEmpty(session.GetString("UserProfileGuid")))
			{
				var userProfileGuid = await userProfileService.GetUserProfileId(userGuid);

				session.SetString("UserProfileGuid", userProfileGuid.ToString());
				return userProfileGuid;
			}
			else
			{
				return Guid.Parse(session.GetString("UserProfileGuid"));
			}
		}

		public static bool IsGuidValid(this Controller controller, string? id, ref Guid productId)
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
