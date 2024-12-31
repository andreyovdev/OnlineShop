namespace OnlineShop.Application.Services.Interfaces
{
    using ViewModels.Shop;

    public interface IProductService
    {
        Task<IEnumerable<AllProductsViewModel>> GetAllProductsAsync();
		Task<IEnumerable<AllProductsViewModel>> GetAllSearchedProductsAsync(string input);
		Task<IEnumerable<AllProductsViewModel>> GetAllProductsPagedAsync(int pageIndex, int pageSize);
		Task<IEnumerable<AllProductsViewModel>> GetAllSearchedProductsPagedAsync(string input, int pageIndex, int pageSize);
        Task<int> GetAllProductsCountAsync();
        Task<int> GetAllSearchedProductsCountAsync(string input);
		Task AddNewProductAsync(AddNewProductViewModel model);
        Task<EditProductViewModel> GetEditProductByIdAsync(Guid productId);
        Task<RemoveProductViewModel> GetRemoveProductByIdAsync(Guid productId);
        Task EditProductAsync(EditProductViewModel model);
        Task RemoveProductAsync(Guid productId);
        Task<ProductDetailsViewModel> GetProductDetailsAsync(Guid productId);
    }
}
