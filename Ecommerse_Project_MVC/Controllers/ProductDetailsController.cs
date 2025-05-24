using AspNetCoreGeneratedDocument;
using Ecommerse_Project_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_Project_MVC.Controllers
{
    public class ProductDetailsController:Controller
    {
        private readonly HttpClient _httpClient;
        public ProductDetailsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(" https://localhost:7019");
        }
        
        public async Task<IActionResult> GetProduct(int? id = null)
        {
            if (id == null)
            {
                // Get the first product if no ID is provided
                var response = await _httpClient.PostAsJsonAsync("api/product/Get_All", new ProductFilterVM());
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ShopPage", "Shop");
                }

                var products = await response.Content.ReadFromJsonAsync<PaginatedResultVM>();
                if (products == null || !products.Products.Any())
                {
                    return RedirectToAction("ShopPage", "Shop");
                }

                var firstProduct = products.Products.First();
                var productDetails = new ProductDetailsVM
                {
                    Id = firstProduct.Id,
                    Name = firstProduct.Name,
                    Description = firstProduct.Description,
                    Price = firstProduct.Price,
                    CategoryName = firstProduct.CategoryName,
                    Images = firstProduct.Images,
                    // Set default values for required fields
                    Stock = 0,
                    AddedAt = DateTime.Now,
                    Brand = "N/A",
                    Material = "N/A",
                    Color = "N/A",
                    Size = "N/A"
                };

                return View(productDetails);
            }

            var product = await _httpClient.GetFromJsonAsync<ProductDetailsVM>($"api/Product/{id}");
            if (product == null)
            {
                return RedirectToAction("ShopPage", "Shop");
            }
            return View(product);
        }

    }
}
