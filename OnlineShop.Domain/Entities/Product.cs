namespace OnlineShop.Domain.Entities
{
    using Enums;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid();
        }

		[Display(AutoGenerateField = false)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public Category Category { get; set; }

        public decimal Price { get; set; }

        public string ImgUrl { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }

		[Display(AutoGenerateField = false)]
		public virtual ICollection<Wishlist> WishlistedByUsers { get; set; } =
            new List<Wishlist>();

		[Display(AutoGenerateField = false)]
		public virtual ICollection<Cart> InCartByUsers { get; set; } =
			new List<Cart>();

		[Display(AutoGenerateField = false)]
		public virtual ICollection<Purchase> PurchasedByUsers { get; set; } =
			new List<Purchase>();
	}
}
