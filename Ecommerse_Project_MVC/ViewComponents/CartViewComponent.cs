using Ecommerse_Project_MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_Project_MVC.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await _cartService.GetCartAsync();
            return View(cart);
        }
    }
} 