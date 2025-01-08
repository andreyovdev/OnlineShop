namespace OnlineShop.Application.Services.Interfaces
{
    using ViewModels.Shop;

    public interface IProductService
    {
        Task<IEnumerable<AllProductsViewModel>> GetAllProductsAsync();
		Task<IEnumerable<AllProductsViewModel>> GetProductsChunkFilteredAsync(FilterOptionsViewModel filter, int chunkIndex, int chuckSize);
		Task<int> GetAllProductsCountAsync();
		Task<int> GetAllFilteredProductCountAsync(FilterOptionsViewModel filter);
		Task<bool> AddNewProductAsync(AddNewProductViewModel model);
        Task<EditProductViewModel> GetEditProductByIdAsync(Guid productId);
        Task<RemoveProductViewModel> GetRemoveProductByIdAsync(Guid productId);
        Task<bool> EditProductAsync(EditProductViewModel model);
        Task<bool> RemoveProductAsync(Guid productId);
        Task<ProductDetailsViewModel> GetProductDetailsAsync(Guid productId);
    }
}
