using System.Net.Http.Headers;
using Ecommerse_Project_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_Project_MVC.Controllers
{
    public class CartController:Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CartController> _logger;
        public CartController(HttpClient httpClient, ILogger<CartController> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(" https://localhost:7019");
            _logger = logger;
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(CartItem cartItem)
        {
            cartItem.Quantity++;
            if (!ModelState.IsValid) return RedirectToAction("ShopPage", "Shop");
    
            var token = Request.Cookies["AuthToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var add_cart = await _httpClient.PostAsJsonAsync<CartItem>("api/Cart", cartItem);

            return RedirectToAction("ShopPage", "Shop");

        }
        public async Task <IActionResult> GetCart()
        {
            if (!ModelState.IsValid) return RedirectToAction("ShopPage", "Shop");

            var token = Request.Cookies["AuthToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var get_cart = await _httpClient.GetFromJsonAsync<CartVM>("api/Cart");
            if(get_cart != null) {return View(get_cart); }
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItemFromCart(int productId)
        {
            try
            {
                var token = Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "UserAuthentication");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync($"/api/Cart/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Item removed from cart successfully";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = error;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while removing the item from cart";
                _logger.LogError(ex, "Error deleting item from cart");
            }

            return RedirectToAction("GetCart");
        }

        [HttpGet]
        public async Task<IActionResult> GetMiniCart()
        {
            try
            {
                var token = Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return PartialView("_MiniCart", new CartVM { cartItems = new List<CartItem>() });
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var cart = await _httpClient.GetFromJsonAsync<CartVM>("api/Cart");
                return PartialView("_MiniCart", cart ?? new CartVM { cartItems = new List<CartItem>() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting mini cart");
                return PartialView("_MiniCart", new CartVM { cartItems = new List<CartItem>() });
            }
        }
    }
}
