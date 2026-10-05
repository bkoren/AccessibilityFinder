using AutoMapper;
using Common.DTOs.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebApp.Models;

public class ProfileController : Controller
{
    private readonly string BaseUrl = "https://localhost:7263/";

    private readonly HttpClient _client;
    private readonly IMapper _mapper;
    public ProfileController(IHttpClientFactory httpClientFactory, IMapper mapper)
    {
        _mapper = mapper;
        _client = httpClientFactory.CreateClient();
    }

    [Authorize]
    public async Task<ActionResult> Profile()
    {
        if (User.Identity?.IsAuthenticated == false)
            return RedirectToAction("Login", "Account");

        string? token = Request.Cookies["JwtToken"];

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        try
        {
            string? id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var response = await _client.GetAsync($"{BaseUrl}user/get/{id}");

            if (response.IsSuccessStatusCode)
            {
                UserReadDTO? user = await response.Content
                    .ReadFromJsonAsync<UserReadDTO>();

                return View(_mapper.Map<ProfileVM>(user));                
            }

            else
            {
                throw new Exception("Unkown error occured!");
            }
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
