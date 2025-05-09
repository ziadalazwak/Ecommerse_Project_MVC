namespace Ecommerse_Project_MVC.Models
{
    public class DashboardResultDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<Product> Products { get; set; }
    }
}
