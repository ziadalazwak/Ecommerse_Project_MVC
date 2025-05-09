using System.ComponentModel.DataAnnotations;

namespace Ecommerse_Project_MVC.Models
{
    public class GetAllProductsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<ImageVM> Images { get; set; }
    }
}
