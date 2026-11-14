using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Common.DTOs.Activity;

namespace WebAPI.IntegrationTests
{
    public class TestActivity : IClassFixture<CustomWebApplicationFactory>
    {
        private HttpClient _client;

        private string adminToken;

        public TestActivity(CustomWebApplicationFactory testFactory)
        {
            _client = testFactory.CreateClient();

            adminToken = CustomWebApplicationFactory.CreateTestToken("admin", 1, "Admin");
        }

        [Fact]
        public async Task TestAddActivity()
        {            
            ActivityCreateDTO input = new ActivityCreateDTO
            {
                Name = "TestActivity",
                Description = "A new test activity for integration testing.",
                Address = "Test street 1, Zagreb",
                Contact = "+385 1 000 0000",
                Email = "testactivity@test.com",
                Type = "Sport",
                Accessibilities = new List<string> { "Wheelchair access" }
            };
         
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            HttpResponseMessage response = await _client.PostAsJsonAsync("activity/add", input);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
