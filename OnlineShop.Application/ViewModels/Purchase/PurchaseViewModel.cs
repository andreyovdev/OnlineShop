namespace OnlineShop.Application.ViewModels.Purchase
{
    public class PurchaseViewModel
    {
        public string ProductId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public int QuantityBought { get; set; }

        public string DatePurchased { get; set; } = null!;
    }
}
