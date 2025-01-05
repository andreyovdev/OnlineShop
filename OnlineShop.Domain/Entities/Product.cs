namespace OnlineShop.Domain.Entities
{
    using Enums;

    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public Category Category { get; set; }

        public decimal Price { get; set; }

        public string ImgUrl { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Wishlist> WishlistedByUsers { get; set; } =
            new List<Wishlist>();

		public virtual ICollection<Cart> InCartByUsers { get; set; } =
			new List<Cart>();

		public virtual ICollection<Purchase> PurchasedByUsers { get; set; } =
			new List<Purchase>();
	}
}
