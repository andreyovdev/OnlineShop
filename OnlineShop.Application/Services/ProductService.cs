﻿namespace OnlineShop.Application.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    
    using Interfaces;
    using Mapping;
	using Domain.Entities;
    using Domain.Enums;
	using ViewModels.Shop;
	using Infrastructure.Data.Repository.Interfaces;
	using System.Linq.Expressions;


    public class ProductService : BaseService, IProductService
    {
		private IRepository<Product> productRepository;

		public ProductService(IRepository<Product> productRepository)
		{
			this.productRepository = productRepository;
		}

        private static TEnum ConvertToEnum<TEnum>(string value) where TEnum : struct
        {
            return Enum.TryParse(value, true, out TEnum result) ? result : default;
        }

        public async Task<IEnumerable<AllProductsViewModel>> GetAllProductsAsync()
        {
            return await this.productRepository
               .GetAllAttached()
               .To<AllProductsViewModel>()
               .Where(p=>p.IsDeleted == false)
               .ToArrayAsync();
        }

		public async Task<IEnumerable<AllProductsViewModel>> GetAllSearchedProductsAsync(string input)
        {
			var allProducts = await this.GetAllProductsAsync();

			return allProducts
				.Where(p => p.Name.ToLower().Contains(input.ToLower()))
				.ToArray();
		}

		public async Task<IEnumerable<AllProductsViewModel>> GetAllProductsPagedAsync(int pageIndex, int pageSize)
		{
			return await this.productRepository
			 .GetAllPagedAttached(pageIndex, pageSize)
			 .To<AllProductsViewModel>()
			 .Where(p => p.IsDeleted == false)
			 .ToArrayAsync();
		}

		public async Task<IEnumerable<AllProductsViewModel>> GetAllSearchedProductsPagedAsync(string input, int pageIndex, int pageSize)
		{
			var searchExpression = string.IsNullOrEmpty(input)
				? (Expression<Func<Product, bool>>)(e => true)
				: e => e.Name.Contains(input);

			return await this.productRepository
			 .GetAllSearchedPagedAttached(searchExpression, pageIndex, pageSize)
			 .To<AllProductsViewModel>()
			 .Where(p => p.IsDeleted == false)
			 .ToArrayAsync();
		}

		public async Task<int> GetAllProductsCountAsync()
        {
            return await this.productRepository.AllCountAsync();
        }

		public async Task<int> GetAllSearchedProductsCountAsync(string input)
		{
			var searchExpression = string.IsNullOrEmpty(input)
				? (Expression<Func<Product, bool>>)(e => true)
				: e => e.Name.Contains(input);

			return await this.productRepository.AllSearchedCountAsync(searchExpression);
		}

		//      public async Task<IEnumerable<AllProductsViewModel>> GetAllProductsFilteredAsync(FilterOptionsViewModel filter)
		//      {
		//          return await products
		//              .Where(p => filter.Categories.Any(c=>c == p.Category))
		//              .to();

		//	// Apply category filter
		//	if (filters.Categories?.Any() == true)
		//	{
		//		products = products.Where(p => filters.Categories.Contains(p.Category)).ToList();
		//	}
		//}

		public async Task AddNewProductAsync(AddNewProductViewModel model)
        {
            Product product = new Product();
            AutoMapperConfig.MapperInstance.Map(model, product);

            product.Category = ConvertToEnum<Category>(model.Category);
            
            productRepository.Add(product);
            await productRepository.SaveAsync();
        }

        public async Task<EditProductViewModel> GetEditProductByIdAsync(Guid productId)
        {
            EditProductViewModel? model = await productRepository
               .GetAllAttached()
               .To<EditProductViewModel>()
               .FirstOrDefaultAsync(p => p.Id.ToLower() == productId.ToString().ToLower());

            return model;
        }

        public async Task EditProductAsync(EditProductViewModel model)
        {
            Product product = new Product();
            AutoMapperConfig.MapperInstance.Map(model, product);

            productRepository.Update(product);
            await productRepository.SaveAsync();
        }

        public async Task<RemoveProductViewModel> GetRemoveProductByIdAsync(Guid productId)
        {
            RemoveProductViewModel? model = await productRepository
              .GetAllAttached()
              .To<RemoveProductViewModel>()
              .FirstOrDefaultAsync(p => p.Id.ToLower() == productId.ToString().ToLower());

            return model;
        }

        public async Task RemoveProductAsync(Guid productId)
        {
            Product product = await productRepository.GetByIdAsync(productId);

            product.IsDeleted = true;

            productRepository.Update(product);
            await productRepository.SaveAsync();
        }

        public async Task<ProductDetailsViewModel> GetProductDetailsAsync(Guid productId)
        {
            return await this.productRepository
            .GetAllAttached()
            .To<ProductDetailsViewModel>()
            .FirstOrDefaultAsync(p => p.Id.ToLower() == productId.ToString().ToLower());
        }

       
    }
}
