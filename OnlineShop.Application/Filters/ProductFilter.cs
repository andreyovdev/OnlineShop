namespace OnlineShop.Application.Filters
{
	using System.Linq.Expressions;
	
	using ViewModels.Shop;
	using Domain.Entities;
	using OnlineShop.Domain.Enums;

	public class ProductFilter
	{
		public static Expression<Func<Product, bool>> CreateFilterPredicate(FilterOptionsViewModel filter)
		{
			Expression<Func<Product, bool>> predicate = p => true;

			if (!string.IsNullOrEmpty(filter.SearchInput))
			{
				string searchInputLower = filter.SearchInput.ToLower();
				predicate = And(predicate, p => p.Name.ToLower().Contains(searchInputLower));
			}

			if (filter.PriceRanges != null && filter.PriceRanges.Any())
			{
				Expression<Func<Product, bool>> priceRangeFilter = p => false;

				if (filter.PriceRanges.Contains("below-200"))
				{
					priceRangeFilter = Or(priceRangeFilter, p => p.Price < 200);
				}
				if (filter.PriceRanges.Contains("201-999"))
				{
					priceRangeFilter = Or(priceRangeFilter, p => p.Price >= 201 && p.Price <= 999);
				}
				if (filter.PriceRanges.Contains("1000-1999"))
				{
					priceRangeFilter = Or(priceRangeFilter, p => p.Price >= 1000 && p.Price <= 1999);
				}
				if (filter.PriceRanges.Contains("above-2000"))
				{
					priceRangeFilter = Or(priceRangeFilter, p => p.Price > 2000);
				}

				predicate = And(predicate, priceRangeFilter);
			}

			if (filter.InStock == true)
			{
				predicate = And(predicate, p => p.Quantity > 0);
			}

			if (filter.Categories != null && filter.Categories.Any())
			{
				var categoryEnumValues = filter.Categories.Select(c => (Category)Enum.Parse(typeof(Category), c)).ToList();

				predicate = And(predicate, p => categoryEnumValues.Contains(p.Category)); 

			}

			return predicate;
		}

		private static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
		{
			var parameter = Expression.Parameter(typeof(T), "p");
			var body = Expression.AndAlso(
				Expression.Invoke(expr1, parameter),
				Expression.Invoke(expr2, parameter)
			);

			return Expression.Lambda<Func<T, bool>>(body, parameter);
		}

		private static Expression<Func<Product, bool>> Or(Expression<Func<Product, bool>> expr1, Expression<Func<Product, bool>> expr2)
		{
			var parameter = Expression.Parameter(typeof(Product), "p");
			var body = Expression.OrElse(
				Expression.Invoke(expr1, parameter),
				Expression.Invoke(expr2, parameter)
			);
			return Expression.Lambda<Func<Product, bool>>(body, parameter);
		}
	}
}
