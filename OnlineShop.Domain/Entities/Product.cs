﻿namespace OnlineShop.Domain.Entities
{
    using Enums;

    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public Category Category { get; set; }

        public string ImgUrl { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }
    }
}
