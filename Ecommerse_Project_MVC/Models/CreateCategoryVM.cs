namespace Ecommerse_Project_MVC.Models
{
    public class CreateCategoryVM
    {
        public string Name { get; set; }

        // This must be provided to indicate whether it's under "Men" or "Women"
        
        public int? ParentCategoryId { get; set; }
    }
}
