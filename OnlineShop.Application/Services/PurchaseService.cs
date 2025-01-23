using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Services.Interfaces;
using OnlineShop.Application.ViewModels.Purchase;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Data.Repository.Interfaces;

namespace OnlineShop.Application.Services
{
    public class PurchaseService : BaseService, IPurchaseService
    {
        private readonly IRepository<UserProfile> userProfileRepository;
        private readonly IRepository<Purchase> purchaseRepository;
        private readonly IRepository<Product> productRepository;

        public PurchaseService(IRepository<Purchase> purchaseRepository, IRepository<UserProfile> userProfileRepository, IRepository<Product> productRepository)
        {
            this.purchaseRepository = purchaseRepository;
            this.userProfileRepository = userProfileRepository;
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<PurchaseViewModel>> GetAllPurchasesAsync(Guid userProfileId)
        {
            var userProfile = await userProfileRepository
                .GetAllAttached()
                .Where(u => u.Id == userProfileId)
                .Include(u => u.Purchases)
                    .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync();

            if (userProfile == null)
            {
                return new List<PurchaseViewModel>();
            }

            var purchases = userProfile.Purchases.Select(p => new PurchaseViewModel
            {
                ProductId = p.ProductId.ToString(),
                Name = p.Product.Name,
                ImgUrl = p.Product.ImgUrl,
                TotalPrice = p.TotalPrice,
                QuantityBought = p.QuantityBought,
                DatePurchased = p.DatePurchased.ToString("yyyy-MM-dd")
            }).ToList();

            return purchases;
        }


        public async Task<bool> CreatePurchase(Guid userProfileId)
        {
            var userProfile = await this.userProfileRepository
                .GetAllAttached()
                .Where(u => u.Id == userProfileId)
                .Include(u => u.Cart)
                    .ThenInclude(c=>c.Product)
                .FirstOrDefaultAsync();

            if (userProfile == null)
            {
                return false;  
            }

            foreach (var cartItem in userProfile.Cart)
            {
                var purchase = new Purchase
                {
                    UserProfileId = userProfileId,
                    ProductId = cartItem.ProductId,
                    QuantityBought = cartItem.Quantity,
                    TotalPrice = cartItem.Quantity * cartItem.Product.Price,
                    DatePurchased = DateTime.UtcNow
                };

                var product = await productRepository
                    .GetAllAttached()
                    .Where(p => p.Id == cartItem.ProductId)
                    .FirstOrDefaultAsync();

                if (product != null)
                {
                    if (product.Quantity >= cartItem.Quantity)
                    {
                        product.Quantity -= cartItem.Quantity;
                        productRepository.Update(product); 
                    }
                    else
                    {
                        return false;
                    }
                }

                purchaseRepository.Add(purchase);
            }

            userProfile.Cart.Clear();

            await purchaseRepository.SaveAsync();
            await productRepository.SaveAsync();

            return true;
        }
    }
}
