namespace Ecommerse_Project_MVC.Models
{
    public class AccountDetailsVM
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }

        public List<AddressVM> Address { get; set; }
    }
}
