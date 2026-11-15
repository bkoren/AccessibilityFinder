using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace WebApp.Controllers
{
    [Route("proxy")]
    public class Proxy : Controller
    {
        private readonly HttpClient _client;          
        private readonly string _baseUrl;

        public Proxy(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _baseUrl = configuration["ApiBaseUrl"]!;
        }

        [Route("{**path}")]
        [AcceptVerbs("GET", "POST", "PUT", "DELETE")]
        public async Task<IActionResult> Forward(string path)
        {
            string apiUrl = _baseUrl + path + Request.QueryString;
                     
            if (!apiUrl.Contains(_baseUrl))
                return BadRequest();

            var request = new HttpRequestMessage(new HttpMethod(Request.Method), apiUrl);

            if (request.Content != null)
            {
                request.Content = new StreamContent(Request.Body);
                request.Content.Headers.ContentType =
                    MediaTypeHeaderValue.Parse(Request.ContentType ?? "application/json");
            }

            string? token = Request.Cookies["JwtToken"];

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = await response.Content.ReadAsStringAsync(),
                ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json"
            };
        }
    }
}
