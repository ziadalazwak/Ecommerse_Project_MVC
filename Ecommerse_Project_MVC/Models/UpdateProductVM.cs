namespace Ecommerse_Project_MVC.Models
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Brand { get; set; }
        public string Material { get; set; }
        
        public int MainCategoryId { get; set; }
       
        public int SubCategoryId { get; set; }
        // Gender-based
        public TargetAudience TargetAudience { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }

    }
}
