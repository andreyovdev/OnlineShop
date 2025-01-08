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
    using Filters;


	public class ProductService : BaseService, IProductService
    {
		private readonly IRepository<Product> productRepository;

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

        public async Task<IEnumerable<AllProductsViewModel>> GetProductsChunkFilteredAsync(FilterOptionsViewModel filter, int chunkIndex, int chuckSize)
        {
			var filterPredicate = ProductFilter.CreateFilterPredicate(filter);
			var orderBy = ProductSorter.CreateSorter(filter);

			return await this.productRepository
			   .GetFilteredChunkAsync(filterPredicate, orderBy, chunkIndex, chuckSize)
			   .To<AllProductsViewModel>()
			   .Where(p => p.IsDeleted == false)
			   .ToArrayAsync();
		}

		public async Task<int> GetAllProductsCountAsync()
        {
            return await this.productRepository.AllCountAsync();
        }

		public async Task<int> GetAllFilteredProductCountAsync(FilterOptionsViewModel filter)
		{
			var filterPredicate = ProductFilter.CreateFilterPredicate(filter);

			return await this.productRepository.AllFilteredCountAsync(filterPredicate);
		}

		public async Task<bool> AddNewProductAsync(AddNewProductViewModel model)
        {
            Product product = new Product();
            AutoMapperConfig.MapperInstance.Map(model, product);

            product.Category = ConvertToEnum<Category>(model.Category);
            
            if(product == null)
            {
                return false;
            }

            productRepository.Add(product);
            await productRepository.SaveAsync();

            return true;
        }

        public async Task<EditProductViewModel> GetEditProductByIdAsync(Guid productId)
        {
            EditProductViewModel? model = await productRepository
               .GetAllAttached()
               .To<EditProductViewModel>()
               .FirstOrDefaultAsync(p => p.Id.ToLower() == productId.ToString().ToLower());

            return model;
        }

        public async Task<bool> EditProductAsync(EditProductViewModel model)
        {
            Product product = new Product();
            AutoMapperConfig.MapperInstance.Map(model, product);

            if(product == null)
            {
                return false;
            }

            productRepository.Update(product);
            await productRepository.SaveAsync();

            return true;
        }

        public async Task<RemoveProductViewModel> GetRemoveProductByIdAsync(Guid productId)
        {
            RemoveProductViewModel? model = await productRepository
              .GetAllAttached()
              .To<RemoveProductViewModel>()
              .FirstOrDefaultAsync(p => p.Id.ToLower() == productId.ToString().ToLower());

            return model;
        }

        public async Task<bool> RemoveProductAsync(Guid productId)
        {
            Product product = await productRepository.GetByIdAsync(productId);

            if(product == null)
            {
                return false;
            }

            product.IsDeleted = true;

            productRepository.Update(product);
            await productRepository.SaveAsync();

            return true;        }

        public async Task<ProductDetailsViewModel> GetProductDetailsAsync(Guid productId)
        {
            return await this.productRepository
            .GetAllAttached()
            .To<ProductDetailsViewModel>()
            .FirstOrDefaultAsync(p => p.Id.ToLower() == productId.ToString().ToLower());
        }

	}
}
