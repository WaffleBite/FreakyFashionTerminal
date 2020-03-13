using System;
using System.Collections.Generic;

namespace FreakyFashionTerminal.Models
{
    class Category
    {
        public Category(string name, Uri image)
        {
            Name = name;
            Image = image;
        }

        public Category(int id, string name, Uri image)
        {
            Id = id;
            Name = name;
            Image = image;
        }

        public Category()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Uri Image { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
