using System.Net.Http.Headers;
using Ecommerse_Project_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_Project_MVC.Controllers
{
    [Authorize]
    public class AccountController:Controller
    {
        private readonly HttpClient _httpClient; 
        private readonly ILogger<AccountController> _logger;
        public AccountController(HttpClient httpClient, ILogger<AccountController> logger)
        {

            _httpClient = httpClient;
           
            _logger = logger; 
            _httpClient.BaseAddress = new Uri("https://localhost:7019");
        }
        [HttpGet]
        public async Task<IActionResult> AccountDetails()
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home");

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var get_userDetails = await _httpClient.GetFromJsonAsync<AccountDetailsVM>("/Api/Account/AccountDetails");

                if (get_userDetails != null)
                    return View(get_userDetails);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "AccountDetails: Failed to fetch account details from API.");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "UserAuthentication");

        }

        [HttpPost("UpdateAccountDetails")]
        public async Task<IActionResult> UpdateAccountDetails(UpdateAccountMV updateAccountMV)
        {
            var token = Request.Cookies["AuthToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var edit_account = await _httpClient.PutAsJsonAsync<UpdateAccountMV>("/Api/Account/UpdateAccount", updateAccountMV);

            if (edit_account.IsSuccessStatusCode)
            {
                return RedirectToAction("AccountDetails", "Account");
            }

            TempData["ErrorMessage"] = "Failed to update account. Please try again.";
            return RedirectToAction("AccountDetails", "Account");

        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePassword)
        {
            var token = Request.Cookies["AuthToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("/api/Account", changePassword);

            // ✅ Try to read the message from the response
            string message;
            try
            {
                message = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                message = "Something went wrong.";
            }

            // ✅ Set message in TempData (success or error, your choice)
            TempData["Message"] = !string.IsNullOrWhiteSpace(message)
                ? message
                : "No response message from server.";

            // ✅ Redirect either way
            return RedirectToAction("AccountDetails", "Account");
        }



    }
}
