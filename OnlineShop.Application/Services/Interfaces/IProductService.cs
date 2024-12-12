
namespace OnlineShop.Application.Services.Interfaces
{
    using ViewModels.Shop;

    public interface IProductService
    {
        Task<IEnumerable<AllProductsViewModel>> GetAllProductsAsync();
        Task<bool> AddNewProductAsync(AddNewProductViewModel model);
    }
}
