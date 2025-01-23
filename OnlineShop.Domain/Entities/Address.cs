namespace OnlineShop.Domain.Entities
{
    public class Address
    {
		public Address()
		{
            this.Id = Guid.NewGuid();
		}
		public Guid Id { get; set; }

        public Guid UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; } = null!;

        public string Country { get; set; } = null!;

		public string City { get; set; } = null!;

		public string Street { get; set; } = null!;

		public string PhoneNumber { get; set; } = null!;
    }
}
