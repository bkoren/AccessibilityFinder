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
        private readonly string BaseUrl = @"https://localhost:7263/activity/";

        private readonly IMapper _mapper;
        private readonly HttpClient _client;

        public ActivityController(IMapper mapper, IHttpClientFactory httpClient)
        {
            _mapper = mapper;
            _client = httpClient.CreateClient();
        }

        public async Task<ActionResult<ActivityVM>> Activity(int id)
        {
            if (User.Identity?.IsAuthenticated == false)
                return RedirectToAction("Login", "Account");

            try
            {
                var response = await _client.GetAsync($"{BaseUrl}get/{id}");

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
