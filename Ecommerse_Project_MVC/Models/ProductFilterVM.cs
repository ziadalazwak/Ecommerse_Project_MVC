namespace Ecommerse_Project_MVC.Models
{
    public class ProductFilterVM
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public List<string>? Sizes { get; set; }
        public List<string>? Colors { get; set; }
        public List<string>? Brands { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 9;
    }
}
