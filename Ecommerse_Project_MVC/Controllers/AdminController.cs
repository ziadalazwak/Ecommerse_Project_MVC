using System.Net.Http.Headers;
using Ecommerse_Project_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_Project_MVC.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
       
        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7019");
    
        }



        public async Task<IActionResult> ControllPage(DashboardPaginationProductsDto dashboard)
        {
            // Build the URL with pageNumber and pageSize as query string parameters
            var url = $"/Api/Product?pageNumber={dashboard.PageNumber}&pageSize={dashboard.PageSize}";

            var get_allProducts = await _httpClient.GetFromJsonAsync<DashboardResultDto>(url);

            if (get_allProducts != null)
                return View(get_allProducts);

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductMV model)
        {
            var token = Request.Cookies["AuthToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (ModelState.IsValid)
            {
                try
                {
                    var formData = new MultipartFormDataContent();

                    formData.Add(new StringContent(model.Name ?? ""), "Name");
                    formData.Add(new StringContent(model.Description ?? ""), "Description");
                    formData.Add(new StringContent(model.Price.ToString()), "Price");
                    formData.Add(new StringContent(model.Stock.ToString()), "Stock");
                    formData.Add(new StringContent(model.Brand ?? ""), "Brand");
                    formData.Add(new StringContent(model.Material ?? ""), "Material");
                    formData.Add(new StringContent(model.Color ?? ""), "Color");
                    formData.Add(new StringContent(model.Size ?? ""), "Size");
                    formData.Add(new StringContent(model.SubCategoryId.ToString()), "SubCategoryId");
                   

                    if (model.Images != null)
                    {
                        foreach (var file in model.Images)
                        {
                            var streamContent = new StreamContent(file.OpenReadStream());
                            formData.Add(streamContent, "Images", file.FileName);
                        }
                    }

                    // Send request
                    var response = await _httpClient.PostAsync("/api/Product", formData);


                    // Handle error response

                    if(response.IsSuccessStatusCode)
                    TempData["Message"] = "Product Added successfully";

                    else
                        TempData["Message"] = "Some thing Went Wrong";
                    return RedirectToAction($"{nameof(ControllPage)}");
                }



                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    TempData["message"] = "Error occurred while adding the product: " + ex.Message;
                    return RedirectToAction("ControllPage");
                }
            }

            // If model state is not valid, return the same view with validation errors
            return RedirectToAction("index");
        }
      
        [HttpGet("Category")]
               public async Task<IActionResult> Category()
                 {
        
                var get_category = await _httpClient.GetFromJsonAsync <List<CategoryVM>>("api/Category");
            if (get_category != null)
                return View(get_category);

                return  RedirectToAction("ControllPage");


             }

        [HttpPost()]
        public async Task<IActionResult> AddCategory(CreateCategoryVM category)
        {

            var get_category = await _httpClient.PostAsJsonAsync<CreateCategoryVM>("api/Category",category);
           if(get_category.IsSuccessStatusCode)
            return RedirectToAction("Category");

            TempData["ErrorMessage"] = "Error occurred while adding the product: ";
            return RedirectToAction("Category");


        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleteProductRequest = await _httpClient.DeleteAsync($"api/Product/{id}");
            string message;
            try
            {
                message = await deleteProductRequest.Content.ReadAsStringAsync();
            }
            catch
            {
                message = "Something went wrong.";
            }

            // ✅ Set message in TempData (success or error, your choice)
            TempData["Message"] = !string.IsNullOrWhiteSpace(message)
                ? message
                : "No response message from server.";
            return RedirectToAction($"{nameof(ControllPage)}");
        }





    }
    }
