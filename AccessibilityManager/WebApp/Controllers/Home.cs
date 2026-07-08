using AutoMapper;
using Common.DTOs.Accessibility;
using Common.DTOs.Type;
using Common.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using WebApp.Models;
using WebApp.Models.Filters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApp.Controllers
{
    public class Home : Controller
    {
        private readonly string BaseUrl = @"https://localhost:7263/";

        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        public Home(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _mapper = mapper;
            _client = httpClientFactory.CreateClient();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == false)
                return RedirectToAction("Login", "Account");

            var getTypes = await _client.GetAsync($"{BaseUrl}type/all");
            
            var getAccessibilities = await _client.GetAsync($"{BaseUrl}accessibility/all");

            List<TypeVM> typeVMs = new();
            List<AccessibilitiesVM> accessibilitiesVMs = new();
            
            try
            {
                if (getTypes.IsSuccessStatusCode)
                {
                    if (getAccessibilities.StatusCode == HttpStatusCode.OK)
                    {
                        List<TypeReadDTO>? types = await getTypes.Content
                        .ReadFromJsonAsync<List<TypeReadDTO>>();

                        if (types != null)
                            typeVMs = _mapper.Map<List<TypeVM>>(types);
                    }
                }
                else
                    throw new Exception(getTypes.StatusCode.ToString());

                if (getAccessibilities.IsSuccessStatusCode)
                {
                    if(getAccessibilities.StatusCode == HttpStatusCode.OK)
                    {
                        List<AccessibilityReadDTO>? accessibilities = await getAccessibilities.Content
                                        .ReadFromJsonAsync<List<AccessibilityReadDTO>>();

                        if (accessibilities != null)
                            accessibilitiesVMs = _mapper.Map<List<AccessibilitiesVM>>(accessibilities);
                    }
                }
                else
                    throw new Exception(getAccessibilities.StatusCode.ToString());


                FiltersVM? filtersVM = new FiltersVM
                {
                    TypeVMs = typeVMs,
                    AccessibilitiesVMs = accessibilitiesVMs
                };

                return View(filtersVM);
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
