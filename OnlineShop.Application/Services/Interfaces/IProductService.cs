
namespace OnlineShop.Application.Services.Interfaces
{
    using ViewModels.Shop;

    public interface IProductService
    {
        Task AddNewProductAsync(AddNewProductViewModel model);
    }
}
