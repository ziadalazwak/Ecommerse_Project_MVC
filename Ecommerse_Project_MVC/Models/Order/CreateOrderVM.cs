namespace Ecommerse_Project_MVC.Models.Order
{
    public class CreateOrderVM
    {
        public int DeliveryMethodId { get; set; }
        public int CartId { get; set; }
        public ShippingAddressVM ShippingAddress { get; set; }
    }
}
