using Ecommerse_Project_MVC.Models;

namespace Ecommerse_Project_MVC.Services
{
    public interface ICartService
    {
        Task<CartVM> GetCartAsync();
    }
} 