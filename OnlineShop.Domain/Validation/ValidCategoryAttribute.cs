namespace OnlineShop.Domain.Validation
{
    using System.ComponentModel.DataAnnotations;
   
    using Enums;
    
    public class ValidCategoryAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is Category category)
            {
                return Enum.IsDefined(typeof(Category), category);
            }
            return false;
        }
    }
}
