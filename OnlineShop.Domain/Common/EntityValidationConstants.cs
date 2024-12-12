namespace OnlineShop.Domain.Common
{
    public class EntityValidationConstants
    {
        public static class Product
        {
            public const int NameMaxLength = 50;
            public const int DescriptionMaxLength = 1000;
            public const string ValidUrlPattern = @"^(http(s):\/\/.)[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)$";
        }
    }
}
