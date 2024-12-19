namespace OnlineShop.Domain.Entities
{
    public class UserProduct
    {
        public Guid UserId { get; set; }

        public virtual UserProfile User { get; set; } = null!;

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
