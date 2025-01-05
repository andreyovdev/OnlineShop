namespace OnlineShop.Domain.Entities
{

    public class UserProfile
    {
        public UserProfile()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

		public Guid AppUserId { get; set; }

		public virtual ICollection<Wishlist> Wishlist { get; set; } =
            new List<Wishlist>();

		public virtual ICollection<Cart> Cart { get; set; } =
		   new List<Cart>();

		public virtual ICollection<Purchase> Purchases { get; set; } =
		   new List<Purchase>();

	}
}
