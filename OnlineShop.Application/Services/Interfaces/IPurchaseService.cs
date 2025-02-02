namespace OnlineShop.Application.Services.Interfaces
{
    using ViewModels.Purchase;
    
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseViewModel>> GetAllPurchasesAsync(Guid userProfileId);
        Task<bool> CreatePurchase(Guid userProfileId);
    }
}
