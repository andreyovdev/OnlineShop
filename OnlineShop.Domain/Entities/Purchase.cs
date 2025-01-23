namespace OnlineShop.Domain.Entities
{
    public class Purchase
    {
        public Purchase()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; } = null!;

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;

		public int QuantityBought { get; set; }

		public decimal TotalPrice { get; set; }

        public DateTime DatePurchased { get; set; }
	}
}
