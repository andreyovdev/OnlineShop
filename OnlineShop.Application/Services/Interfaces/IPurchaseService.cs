using OnlineShop.Application.ViewModels.Purchase;

namespace OnlineShop.Application.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseViewModel>> GetAllPurchasesAsync(Guid userProfileId);
        Task<bool> CreatePurchase(Guid userProfileId);
    }
}
