using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Ecommerse_Project_MVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }

        public DateTime AddedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public string Brand { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string AdminId { get; set; }
        public string AdminName { get; set; }

        public List<ImageVM> ImageUrls { get; set; } = new();

    }
}
