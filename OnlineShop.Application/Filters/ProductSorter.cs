namespace OnlineShop.Application.Filters
{
	using OnlineShop.Application.ViewModels.Shop;
	using OnlineShop.Domain.Entities;
	using OnlineShop.Domain.Enums;
	using System.Linq.Expressions;

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
