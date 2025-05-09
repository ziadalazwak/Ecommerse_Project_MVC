using System.Net.Http;
using Ecommerse_Project_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_Project_MVC.Controllers
{
    public class ShopController:Controller
    {
        private readonly HttpClient httpClient;
        public ShopController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://localhost:7019");
        }
        public async Task<IActionResult> ShopPage(ProductFilterVM filter)
        {
            // Ensure filter values are set (fallback to defaults if necessary)
         
            
            
            // Get categories
            var categories = await httpClient.GetFromJsonAsync<List<CategoryVM>>("api/Category");

            // Send the filter to the API to get the filtered products
            var response = await httpClient.PostAsJsonAsync("api/product/Get_All", filter);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error (log or show message)
                return View("Error");
            }

            // Get paginated result
            var paginatedResult = await response.Content.ReadFromJsonAsync<PaginatedResultVM>();

            // Prepare the view model
            var viewModel = new CategoryProductVM
            {
                Categories = categories,
                Products = paginatedResult.Products,
                SelectedCategoryId = filter.CategoryId,
                Filter = filter,  // Pass the filter back to the view
                CurrentPage = filter.PageNumber.Value,
                TotalPages = (int)Math.Ceiling((double)paginatedResult.TotalCount / filter.PageSize.Value)
              
            };

            return View(viewModel);
        }
        [HttpGet("ProductBox/{id}")]
        public async Task<IActionResult> ProductBox(int id)
        {
            var product = await httpClient.GetFromJsonAsync<GetAllProductsVM>($"api/product/{id}");
            if (product != null)
            {
                return View(product);
            }
            return RedirectToAction("ShopPage");
        }




    }
}

