namespace Ecommerse_Project_MVC.Models
{
    public class CartVM
    {

        public int Id { get; set; }
        public string CustomerId { get; set; }
        public ICollection<CartItem> cartItems { get; set; }
        public decimal TotalPrice => cartItems != null ? cartItems.Sum(i => i.Price * i.Quantity) : 0;
    }
}
