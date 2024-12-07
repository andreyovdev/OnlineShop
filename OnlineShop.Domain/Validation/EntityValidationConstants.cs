namespace OnlineShop.Domain.Validation
{
    public class EntityValidationConstants
    {
        public static class Product
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 100;

            public const int CategoryMinLength = 3;
            public const int CategoryMaxLength = 50;

            public const int DescriptionMaxLength = 1000;
        }
    }
}
