namespace OnlineShop.Application.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    
    using Interfaces;
    using Mapping;
	using Domain.Entities;
    using Domain.Enums;
	using ViewModels.Shop;
	using Infrastructure.Data.Repository.Interfaces;

    public class ProductService : BaseService, IProductService
    {
		private IRepository<Product> productRepository;

		public ProductService(IRepository<Product> productRepository)
		{
			this.productRepository = productRepository;
		}

        public async Task<IEnumerable<AllProductsViewModel>> GetAllProductsAsync()
        {
            return await this.productRepository
               .GetAllAttached()
               .To<AllProductsViewModel>()
               .ToArrayAsync();
        }

        public async Task<bool> AddNewProductAsync(AddNewProductViewModel model)
        {
            Product product = new Product();
            AutoMapperConfig.MapperInstance.Map(model, product);
            product.Category = ConvertToEnum<Category>(model.Category);
            
            this.productRepository.Add(product);
            await this.productRepository.SaveAsync();

            return true;
        }

        private static TEnum ConvertToEnum<TEnum>(string value) where TEnum : struct
        {
            return Enum.TryParse(value, true, out TEnum result) ? result : default;
        }
    }
}
