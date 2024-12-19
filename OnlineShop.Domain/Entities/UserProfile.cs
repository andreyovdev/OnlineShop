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

        public virtual ICollection<UserProduct> UserProducts { get; set; } =
            new HashSet<UserProduct>();
    }
}
