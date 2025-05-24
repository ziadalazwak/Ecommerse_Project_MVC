using System.Security.Claims;
using Ecommerse_Project_MVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace Ecommerse_Project_MVC.Controllers
{
    public class UserAuthenticationController:Controller
    {
        private readonly HttpClient _httpClient; 
        public UserAuthenticationController(HttpClient httpClient)
        {
   
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(" https://localhost:7019");
        }
        [HttpGet]
        public  IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAction(LoginVM loginVM)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("/Api/UserAuthentication/Login", loginVM);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadFromJsonAsync<AuthResponse>();

                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await SignInUserFromJwtAsync(result.Token);

                    // You can now check the token manually if needed
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(result.Token);
                    var claims = jwtToken.Claims;

                    if (claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin"))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", result?.Message ?? "Login failed");
                return View(loginVM);
            }

            ModelState.AddModelError("", "Server error. Please try again.");
            return View(loginVM);
        }


        private async Task SignInUserFromJwtAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToList();

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            // Store the JWT token as a HttpOnly cookie
            HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Strict,
                IsEssential = true
            });
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAction(RegisterVM register)
        {
            var respose = await _httpClient.PostAsJsonAsync<RegisterVM>("Api/UserAuthentication/Register", register);
            if (respose.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");


            }
            var result = await respose.Content.ReadFromJsonAsync<AuthResponse>();

            if (result != null)
            {
                ModelState.AddModelError("", result?.Message ?? "Login failed");
            }
            return View(register);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out from cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Optionally, delete the token cookie if you're storing it manually
            HttpContext.Response.Cookies.Delete("AuthToken");

            // Redirect to login page after logout
            return RedirectToAction("Login");
        }

    }
}
