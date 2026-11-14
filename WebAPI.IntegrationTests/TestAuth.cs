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
    public class TestAuth : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;    

        public TestAuth(CustomWebApplicationFactory testFactory)
        {
            _client = testFactory.CreateClient();            
        }


        [Fact]
        public async Task TestAdminAccessWithNoToken()
        {           
            var response = await _client.GetAsync("/log/count");
           
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task TestAdminAccessWithUserToken()
        {    
            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", CustomWebApplicationFactory.CreateTestToken("user", 2));
         
            var response = await _client.GetAsync("/log/count");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task TestAdminAccessWithAdminToken()
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", CustomWebApplicationFactory.CreateTestToken("admin", 1, "Admin"));

            var response = await _client.GetAsync("/log/count");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestLogin()
        {
            var loginData = new
            {
                Username = "admin",
                Password = "admin123"
            };

            var response = await _client.PostAsJsonAsync("/account/login", loginData);     
   
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            Assert.NotNull(content);
            Assert.True(content.ContainsKey("token"));
        }

        [Fact]
        public async Task TestRegistration()
        {            
            var registerData = new
            {
                Username = "newuser",
                Password = "newuser123",
                ConfirmPassword = "newuser123",
                FirstName = "New",
                LastName = "User",
                Email = "newuser@example.com"
            };

            var registerResponse = await _client.PostAsJsonAsync("/account/register", registerData);

            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);
         
            var loginData = new
            {
                Username = "newuser",
                Password = "newuser123"
            };

            var loginResponse = await _client.PostAsJsonAsync("/account/login", loginData);

            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var content = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            Assert.NotNull(content);
            Assert.True(content.ContainsKey("token"));
        }
    }
}
