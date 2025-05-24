using System.Net.Http.Headers;
using Ecommerse_Project_MVC.Models;

namespace Ecommerse_Project_MVC.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7019");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CartVM> GetCartAsync()
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return new CartVM { cartItems = new List<CartItem>() };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var cart = await _httpClient.GetFromJsonAsync<CartVM>("api/Cart");
                return cart ?? new CartVM { cartItems = new List<CartItem>() };
            }
            catch
            {
                return new CartVM { cartItems = new List<CartItem>() };
            }
        }
    }
} 