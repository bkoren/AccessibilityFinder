using AutoMapper;
using System.Text.Json;
using Common.DTOs.Activity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Activity;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ActivityController : Controller
    {
        private readonly string _baseUrl;

        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public ActivityController(IMapper mapper, IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _mapper = mapper;
            _client = httpClient.CreateClient();
            _configuration = configuration;

            _baseUrl = _configuration["ApiBaseUrl"]!;
        }

        public async Task<ActionResult<ActivityVM>> Activity(int id)
        {
            if (User.Identity?.IsAuthenticated == false)
                return RedirectToAction("Login", "Account");

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}activity/get/{id}");

                string? token = Request.Cookies["JwtToken"];
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return View(await response.Content.ReadFromJsonAsync<ActivityVM>());
                }
                else
                    throw new Exception(response.StatusCode.ToString());    

            }
            catch (Exception error)
            {
                ErrorVM model = new ErrorVM
                {
                    Error = error.Message,
                };

                return View("~/Views/Shared/Error.cshtml", model);
            }                      
        }
    }
}
