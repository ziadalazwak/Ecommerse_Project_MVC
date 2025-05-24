namespace Ecommerse_Project_MVC.Models
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime AddedAt { get; set; }
        public string Brand { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ImageVM> Images { get; set; }
    }
}
