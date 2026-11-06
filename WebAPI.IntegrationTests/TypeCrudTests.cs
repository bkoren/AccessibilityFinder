using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Common.DTOs.Type;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests
{
    public class TypeCrudTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutput;
        private readonly CustomWebApplicationFactory _factory;

        public TypeCrudTests(CustomWebApplicationFactory factory, ITestOutputHelper testOutput)
        {
            _client = factory.CreateClient();         
            _factory = factory;
            
            _testOutput = testOutput;
        }

        [Fact]
        public async Task Add_Type_Returns_Ok()
        {
            TypeCreateDTO input = new TypeCreateDTO
            {
                Name = "TestType",
            };

            HttpClient adminClient = _factory.CreateAdminClient();


            HttpResponseMessage response = await adminClient.PostAsJsonAsync("type/add", input);

            _testOutput.WriteLine(await response.Content.ReadAsStringAsync());

            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"\u001b[FAILED]\u001b Expected: {HttpStatusCode.OK}, Actual: {response.StatusCode}");                 
        }    

        [Fact]
        public async Task Add_Duplicate_Type_Returns_BadRequest()
        {
            var input = new TypeCreateDTO
            {
                Name = "TestType",
            };

            await _client.PostAsJsonAsync("type/add", input);

            HttpResponseMessage response = await _client.PostAsJsonAsync("type/add", input);

            var expectedStatus = HttpStatusCode.BadRequest;
            Assert.Equal(expectedStatus, response.StatusCode);

            string body = await response.Content.ReadAsStringAsync();
            var expectedMessage = "Type already exists!";
            Assert.Contains(expectedMessage, body);
        }

        [Fact]
        public async Task Get_All_Types_Returns_Expected()
        {
            HttpResponseMessage response = await _client.GetAsync("type/all");
          
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);



            if (response.StatusCode == HttpStatusCode.OK)
            {
                var types = await response.Content.ReadFromJsonAsync<List<TypeReadDTO>>();
                Assert.NotNull(types);               

                var expectedName = "TestType";
                Assert.Contains(types, t => t.Name == expectedName);
            }
        }

        [Fact]
        public async Task Delete_Type_Returns_Expected()
        {
            string testToken = _factory.GetTestJwtToken("TestAdmin", 1, "Admin", 60);

            var idToDelete = 0;

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", testToken);
            HttpResponseMessage response = await _client.DeleteAsync($"type/delete/{idToDelete}");

            var expectedStatus = HttpStatusCode.OK;
            Assert.Equal(expectedStatus, response.StatusCode);
        }      
    }
}
