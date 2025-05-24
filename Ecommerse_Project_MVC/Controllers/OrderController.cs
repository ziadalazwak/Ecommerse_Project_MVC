using Ecommerse_Project_MVC.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Ecommerse_Project_MVC.Models.Order;

namespace Ecommerse_Project_MVC.Controllers
{
    public class OrderController:Controller
    {
        private readonly HttpClient _httpClient;
        public OrderController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(" https://localhost:7019");
        }
        [HttpGet]
        public async Task<IActionResult> OrderPage()
        {
            if (!ModelState.IsValid) return RedirectToAction("ShopPage", "Shop");

            var token = Request.Cookies["AuthToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var get_cart = await _httpClient.GetFromJsonAsync<CartVM>("api/Cart");
            if (get_cart == null) return RedirectToAction("Account", "AccountDetails");
            var get_userDetails = await _httpClient.GetFromJsonAsync<AccountDetailsVM>("/Api/Account/AccountDetails");
            if (get_userDetails == null) return RedirectToAction("Account", "AccountDetails");
            var orderDetails = new OrderPageVM
            {
                cart = get_cart,
                accountDetails = get_userDetails
            };
            if (orderDetails != null) { return View(orderDetails); }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressVM address)
        {
            try
            {
                var token = Request.Cookies["AuthToken"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("/Api/Account/AddAddress", address);
                
                if (response.IsSuccessStatusCode)
                {
                   return Redirect (nameof(OrderPage));
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return Redirect(nameof(OrderPage));
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderVM orderDetails)
        {
            try
            {
                var token = Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "UserAuthentication");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("/Api/Order/CreateOrder", orderDetails);
                
                if (response.IsSuccessStatusCode)
                {
                    // Clear the cart after successful order creation
                    await _httpClient.DeleteAsync("/Api/Cart/ClearCart");
                    return RedirectToAction("OrderConfirmation", new { orderId = await response.Content.ReadFromJsonAsync<int>() });
                }
                else
                {
                    TempData["Error"] = "We are working on it. Please try again later.";
                    return RedirectToAction("OrderPage");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "We are working on it. Please try again later.";
                return RedirectToAction("OrderPage");
            }
        }
    }
}
