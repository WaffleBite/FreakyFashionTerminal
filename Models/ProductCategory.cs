namespace FreakyFashionTerminal.Models
{
    class ProductCategory
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}