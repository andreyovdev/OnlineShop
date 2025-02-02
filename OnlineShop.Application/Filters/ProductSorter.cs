namespace OnlineShop.Application.Filters
{
	using System.Linq.Expressions;

	using ViewModels.Shop;
	using Domain.Entities;

	public class ProductSorter
	{
		public static List<(Expression<Func<Product, object>> orderBy, bool isDescending)> CreateSorter(FilterOptionsViewModel filter)
		{
			var sorter = new List<(Expression<Func<Product, object>> orderBy, bool isDescending)>();

			if (!string.IsNullOrEmpty(filter.SortByPrice))
			{
				Expression<Func<Product, object>> priceOrderBy = p => p.Price;
				bool isDescending = filter.SortByPrice.Equals("descending", StringComparison.OrdinalIgnoreCase);

				sorter.Add((priceOrderBy, isDescending));
			}

			return sorter;
		}
	}
}
