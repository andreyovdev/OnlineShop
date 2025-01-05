﻿namespace OnlineShop.Domain.Entities
{
    public class Purchase
    {
        public Guid UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; } = null!;

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;

		public int Quantity { get; set; }

		public DateTime DatePurchased { get; set; }
	}
}
