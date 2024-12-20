namespace OnlineShop.Application.Services.Interfaces
{
    using ViewModels.Shop;

    public interface IProductService
    {
        Task<IEnumerable<AllProductsViewModel>> GetAllProductsAsync();
        Task<IEnumerable<AllProductsViewModel>> SearchProductsAsync(string input);
        Task AddNewProductAsync(AddNewProductViewModel model);
        Task<EditProductViewModel> GetEditProductByIdAsync(Guid productId);
        Task<RemoveProductViewModel> GetRemoveProductByIdAsync(Guid productId);
        Task EditProductAsync(EditProductViewModel model);
        Task RemoveProductAsync(Guid productId);
        Task<ProductDetailsViewModel> GetProductDetailsAsync(Guid productId);
    }
}
