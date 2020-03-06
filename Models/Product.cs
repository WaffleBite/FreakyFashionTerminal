using System;
using System.Collections.Generic;

namespace FreakyFashionTerminal.Models
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArtNumber { get; set; }
        public decimal Price { get; set; }
        public Uri ImageUrl { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
