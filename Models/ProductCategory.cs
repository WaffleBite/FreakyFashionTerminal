namespace FreakyFashionTerminal.Models
{
    class ProductCategory
    {
        public ProductCategory(int productId, int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}