using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Common.DTOs.Type;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests
{
    public class TestType : IClassFixture<CustomWebApplicationFactory>
    {                     
        private HttpClient _client;

        private string adminToken;

        public TestType(CustomWebApplicationFactory testFactory)
        {                                   
            _client = testFactory.CreateClient();

            adminToken = CustomWebApplicationFactory.CreateTestToken("admin", 1, "Admin");
        }

        [Fact]
        public async Task Add_Type_Returns_Ok()
        {
            TypeCreateDTO input = new TypeCreateDTO
            {
                Name = "TestType",
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            HttpResponseMessage response = await _client.PostAsJsonAsync("type/add", input);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }    


        [Fact]
        public async Task Get_All_Types_Returns_Expected()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            HttpResponseMessage response = await _client.GetAsync("type/all");
          
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);         
        }        
    }
}
