﻿using System.ComponentModel.DataAnnotations;

namespace Ecommerse_Project_MVC.Models
{
    public class AddProductMV
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Brand { get; set; }
        public string Material { get; set; }
        [Required]
        public int MainCategoryId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        // Gender-based
        public TargetAudience TargetAudience { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        //public string AdminID { get; set; }
        public IFormFileCollection Images { get; set; }
    }
}
