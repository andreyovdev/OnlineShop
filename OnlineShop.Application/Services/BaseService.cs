namespace OnlineShop.Application.Services
{
	using Interfaces;

	public class BaseService : IBaseService
	{
		public bool IsGuidValid(string? id, ref Guid parseGuid)
		{
			//Invalid parameter in URL
			if (string.IsNullOrWhiteSpace(id))
			{
				return false;
			}

			//Invalid id in URL
			Guid validatedGuid = Guid.Empty;
			bool isGuidValid = Guid.TryParse(id, out validatedGuid);
			if (!isGuidValid)
			{
				return false;
			}

			parseGuid = validatedGuid;

			return true;
		}
	}
}
