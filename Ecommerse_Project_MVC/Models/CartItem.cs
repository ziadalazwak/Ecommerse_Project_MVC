namespace Ecommerse_Project_MVC.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } // Optional
        public string? ImageUrl { get; set; }
       
        public decimal Price { get; set; } // Unit price
        public int Quantity { get; set; }
    }
}
