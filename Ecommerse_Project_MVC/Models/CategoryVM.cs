using System.ComponentModel.DataAnnotations;

namespace Ecommerse_Project_MVC.Models
{
    public class CategoryVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<SubCategoryDto>? SubCategories { get; set; } = new List<SubCategoryDto>();
    }
}
