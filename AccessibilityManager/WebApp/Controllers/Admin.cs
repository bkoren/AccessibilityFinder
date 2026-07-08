using AutoMapper;
using Common.DTOs.Accessibility;
using Common.DTOs.Activity;
using Common.DTOs.Log;
using Common.DTOs.Review;
using Common.DTOs.Type;
using Common.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using WebApp.Models;
using WebApp.Models.Admin;
using WebApp.Models.Admin.SendModels;

namespace WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly string BaseUrl = @"https://localhost:7263/";

        private readonly IMapper _mapper;
        private readonly HttpClient _client;

        public AdminController(IHttpClientFactory httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _client = httpClient.CreateClient();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateType([Bind("Type")] AdminVM model)
        {
            AdminVM adminVM = await LoadAdminVM();

            adminVM.Type = model.Type;

            string? token = Request.Cookies["JwtToken"];

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync(
                $"{BaseUrl}type/add", model.Type
            );

            var url = Url.Action("Admin", adminVM) + "#type-section";

            if (response.IsSuccessStatusCode)
            {
                TempData["TypeSuccess"] = "Type created successfully.";

                return Redirect(url);
            }

            else
            {
                string error = await response.Content.ReadAsStringAsync();

                try
                {
                    var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(error);

                    var messages = problem!.Errors
                        .SelectMany(e => e.Value)
                        .ToList();

                    TempData["TypeError"] = string.Join(", ", messages);

                    return Redirect(url);
                }
                catch (Exception)
                {
                    TempData["TypeError"] = error;

                    return Redirect(url);
                }
            }
        }
        

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateAccessibility([Bind("Accessibility")] AdminVM model)
        {
            AdminVM adminVM = await LoadAdminVM();

            adminVM.Accessibility = model.Accessibility;

            string? token = Request.Cookies["JwtToken"];

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync(
                $"{BaseUrl}accessibility/add", model.Accessibility
            );

            var url = Url.Action("Admin", adminVM) + "#accessibility-section";

            if (response.IsSuccessStatusCode)
            {
                TempData["AccessibilitySuccess"] = "Accessibility added.";
                   
                return Redirect(url);
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();

                try
                {
                    var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(error);

                    var messages = problem!.Errors
                        .SelectMany(e => e.Value)
                        .ToList();


                    TempData["AccessibilityError"] = string.Join(", ", messages);

                    return Redirect(url);
                }
                catch (Exception)
                {

                    TempData["AccessibilityError"] = error;

                    return Redirect(url);
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateActivity([Bind("Activity")] AdminVM model)
        {
            AdminVM adminVM = await LoadAdminVM();
  
            adminVM.Activity = model.Activity;

            string? token = Request.Cookies["JwtToken"];

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync(
                $"{BaseUrl}activity/add", model.Activity                
            );

            var url = Url.Action("Admin", adminVM) + "#activity-section";

            if (response.IsSuccessStatusCode)
            {               

                TempData["ActivitySuccess"] = "Activity created successfully.";

                return Redirect(url);                
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();

                try
                {
                    var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(error);

                    var messages = problem!.Errors
                        .SelectMany(e => e.Value)
                        .ToList();
                   
                    TempData["ActivityError"] = string.Join(", ", messages);

                    return View("Admin", adminVM);
                }
                catch (Exception)
                {

                    TempData["ActivityError"] = error;

                    return Redirect(url);
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateActivity([Bind("Update")] AdminVM model)
        {
            AdminVM adminVM = await LoadAdminVM();

            adminVM.Update = model.Update;

            string? token = Request.Cookies["JwtToken"];

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJsonAsync(
                $"{BaseUrl}activity/update{model.Update.Id}", model.Update
            );

            var url = Url.Action("Admin", adminVM);

            if (response.IsSuccessStatusCode)
            {
                TempData["ActivitySuccess"] = "Activity updated successfully.";

                return Redirect(url);
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();

                try
                {
                    var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(error);

                    var messages = problem!.Errors
                        .SelectMany(e => e.Value)
                        .ToList();

                    TempData["ActivityError"] = string.Join(", ", messages);

                    return Redirect(url);
                }
                catch (Exception)
                {
                    TempData["ActivityError"] = error;

                    return Redirect(url);
                }
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Admin()
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            try
            {
                AdminVM adminVM = await LoadAdminVM();

                return View(adminVM);
            }
            catch (Exception error)
            {
                ErrorVM message = new ErrorVM
                {
                    Error = error.Message,
                };

                return View("~/Views/Shared/Error.cshtml", message);
            }
        }

        private async Task<AdminVM> LoadAdminVM()
        {
            string? token = Request.Cookies["JwtToken"];

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            AdminVM adminVM = new AdminVM();

            var getAllActivities = await _client.GetAsync($"{BaseUrl}activity/all");
            if (getAllActivities.IsSuccessStatusCode)
            {
                if (getAllActivities.StatusCode != HttpStatusCode.NoContent && getAllActivities.StatusCode != HttpStatusCode.NotFound)
                {
                    var activities = await getAllActivities.Content.ReadFromJsonAsync<List<ActivityGetDTO>>();
                    
                    adminVM.ActivityAdminVMs = _mapper.Map<List<ActivityAdminVM>>(activities);
                }
            }

            var getAllReviews = await _client.GetAsync($"{BaseUrl}review/all");
            if (getAllReviews.IsSuccessStatusCode)
            {
                if (getAllReviews.StatusCode != HttpStatusCode.NoContent && getAllReviews.StatusCode != HttpStatusCode.NotFound)
                {
                    var reviews = await getAllReviews.Content.ReadFromJsonAsync<List<ReviewReadDTO>>();

                    adminVM.ReviewAdminVMs = _mapper.Map<List<ReviewAdminVM>>(reviews);
                }
            }

            var getAllUsers = await _client.GetAsync($"{BaseUrl}user/all");
            if (getAllUsers.IsSuccessStatusCode)
            {
                if (getAllUsers.StatusCode != HttpStatusCode.NoContent && getAllUsers.StatusCode != HttpStatusCode.NotFound)
                {
                    var users = await getAllUsers.Content.ReadFromJsonAsync<List<UserReadDTO>>();

                    adminVM.UsersAdminVMs = _mapper.Map<List<UsersAdminVM>>(users);
                }

            }

            var getAllLogs = await _client.GetAsync($"{BaseUrl}log/all");
            if (getAllLogs.IsSuccessStatusCode)
            {
                if (getAllLogs.StatusCode != HttpStatusCode.NoContent && getAllLogs.StatusCode != HttpStatusCode.NotFound)
                {
                    var logs = await getAllLogs.Content.ReadFromJsonAsync<List<LogReadDTO>>();

                    adminVM.LogsAdminVMs = _mapper.Map<List<LogsAdminVM>>(logs);
                }
            }

            var getAllTypes = await _client.GetAsync($"{BaseUrl}type/all");
            if (getAllTypes.IsSuccessStatusCode)
            {
                if (getAllTypes.StatusCode != HttpStatusCode.NoContent && getAllTypes.StatusCode != HttpStatusCode.NotFound)
                {
                    var types = await getAllTypes.Content.ReadFromJsonAsync<List<TypeReadDTO>>();

                    adminVM.TypeAdminVMs = _mapper.Map<List<TypeAdminVM>>(types);
                }
            }

            var getAllaccessibilites = await _client.GetAsync($"{BaseUrl}accessibility/all");
            if (getAllaccessibilites.IsSuccessStatusCode)
            {
                if (getAllaccessibilites.StatusCode != HttpStatusCode.NoContent && getAllaccessibilites.StatusCode != HttpStatusCode.NotFound)
                {
                    var accessibilities = await getAllaccessibilites.Content.ReadFromJsonAsync<List<AccessibilityReadDTO>>();

                    adminVM.AccessibilityAdminVMs = _mapper.Map<List<AccessibilityAdminVM>>(accessibilities);
                }
            }

            return adminVM;
        }
    }
}
