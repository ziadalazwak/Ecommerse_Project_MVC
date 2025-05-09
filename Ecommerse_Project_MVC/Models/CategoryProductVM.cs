namespace Ecommerse_Project_MVC.Models
{

    
    
        public class CategoryProductVM
        {
            public ICollection<CategoryVM>? Categories { get; set; }
            public ICollection<GetAllProductsVM> Products { get; set; }
            public int? SelectedCategoryId { get; set; }

            // Add the filter properties to pass them to the view
            public ProductFilterVM Filter { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }

    }
    }


