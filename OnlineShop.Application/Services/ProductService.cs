namespace OnlineShop.Application.Services
{
    using System.Threading.Tasks;

    using Interfaces;
	using ViewModels.Shop;
	using Domain.Entities;
	using Infrastructure.Data.Repository.Interfaces;
    using Mapping;
    using OnlineShop.Domain.Enums;

    public class ProductService : BaseService, IProductService
    {
		private IRepository<Product> productRepository;

		public ProductService(IRepository<Product> productRepository)
		{
			this.productRepository = productRepository;
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
