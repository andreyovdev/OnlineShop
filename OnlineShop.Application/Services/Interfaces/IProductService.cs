
namespace OnlineShop.Application.Services.Interfaces
{
    using ViewModels.Shop;

    public interface IProductService
    {
        Task<bool> AddNewProductAsync(AddNewProductViewModel model);
    }
}
