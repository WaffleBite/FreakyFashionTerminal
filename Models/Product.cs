using System;
using System.Collections.Generic;

namespace FreakyFashionTerminal.Models
{
    class Product
    {
        public Product(string name, string description, string artNumber, decimal price, Uri imageUrl)
        {
            Name = name;
            Description = description;
            ArtNumber = artNumber;
            Price = price;
            ImageUrl = imageUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArtNumber { get; set; }
        public decimal Price { get; set; }
        public Uri ImageUrl { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
