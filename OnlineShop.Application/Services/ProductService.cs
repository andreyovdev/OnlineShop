namespace OnlineShop.Application.Services
{
    using System;
    using System.Threading.Tasks;

    using Interfaces;
	using ViewModels.Shop;
	using Domain.Entities;
	using Infrastructure.Data.Repository.Interfaces;

	public class ProductService : BaseService, IProductService
    {
		private IRepository<Product> productRepository;

		public ProductService(IRepository<Product> productRepository)
		{
			this.productRepository = productRepository;
		}

		public async Task AddNewProductAsync(AddNewProductViewModel model)
        {
            throw new NotImplementedException();
        }

    }
}
