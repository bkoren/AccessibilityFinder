using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class Account : Controller
    {
        private readonly HttpClient _client;
        public Account(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("JwtToken"))
            {
                Response.Cookies.Delete("JwtToken");
            }

            return RedirectToAction("Landing", "Landing");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if(!ModelState.IsValid) 
                return View(model);

            var response = await _client.PostAsync(
                "https://localhost:7263/Account/Login",

                new StringContent(
                    JsonSerializer.Serialize(model),
                    Encoding.UTF8, "application/json"
                )
            );

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                CookieOptions options = new CookieOptions
                {
                    Secure = true,                    
                    Expires = DateTime.Now.AddMinutes(20)
                };

                string token = JsonDocument
                    .Parse(json)
                    .RootElement
                    .GetProperty("token")
                    .GetString()!;

                Response.Cookies.Append("JwtToken", token, options);
                             
                return RedirectToAction("Index", "Home");

            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();

                string message = JsonDocument
                    .Parse(error)
                    .RootElement
                    .GetProperty("message")
                    .GetString()!;

                ModelState.AddModelError(string.Empty, "Unsuccessful login: " + message);
                
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _client.PostAsync(
                "https://localhost:7263/Account/Register",

                new StringContent(
                    JsonSerializer.Serialize(model),
                    Encoding.UTF8, "application/json"
                )
            );

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();

                string message = JsonDocument
                    .Parse(error)
                    .RootElement
                    .GetProperty("message")
                    .GetString()!;

                ModelState.AddModelError(string.Empty, "Unsuccessful registration: " + message);
            }

            return View(model);
        }
    }
}
