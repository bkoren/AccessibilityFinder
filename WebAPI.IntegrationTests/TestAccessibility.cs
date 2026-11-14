using Common.DTOs.Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.IntegrationTests
{
    public class TestAccessibility : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;        

        public TestAccessibility(CustomWebApplicationFactory testFactory)
        {
            _client = testFactory.CreateClient();            
        }

        [Fact]
        public async Task TestGetAccessibility()
        {
            var response = await _client.GetAsync("/accessibility/all");

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


        [Fact]
        public async Task TestAddAccessibility()
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CustomWebApplicationFactory.CreateTestToken("admin", 1, "Admin"));

            AccessibilityCreateDTO accessibility = new AccessibilityCreateDTO
            {
                Name = "testAccessibility"
            };


            var response = await _client.PostAsJsonAsync("/accessibility/add", accessibility);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
