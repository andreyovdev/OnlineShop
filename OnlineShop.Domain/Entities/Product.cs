﻿namespace OnlineShop.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    
    using Enums;
    using Validation;
    using static Validation.EntityValidationConstants.Product;

    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [ValidCategory]
        public Category Category { get; set; }

        [Required]
        [Url]
        public string ImgUrl { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }
    }
}
