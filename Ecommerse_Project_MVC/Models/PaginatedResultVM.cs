namespace Ecommerse_Project_MVC.Models
{
    public class PaginatedResultVM
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public ICollection<GetAllProductsVM> Products { get; set; }
    }
}
