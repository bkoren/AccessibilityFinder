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
        private readonly ITestOutputHelper _testOutput;
        private readonly CustomWebApplicationFactory _factory;

        public TypeCrudTests(CustomWebApplicationFactory factory, ITestOutputHelper testOutput)
        {            
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

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }    

        [Fact]
        public async Task Add_Duplicate_Type_Returns_BadRequest()
        {
            TypeCreateDTO input = new TypeCreateDTO
            {
                Name = "TestType",
            };

            HttpClient adminClient = _factory.CreateAdminClient();
            HttpResponseMessage response = await adminClient.PostAsJsonAsync("type/add", input);

            _testOutput.WriteLine(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);          
            Assert.Contains("Type already exists", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Get_All_Types_Returns_Expected()
        {
            HttpClient userClient = _factory.CreateClient();
            HttpResponseMessage response = await userClient.GetAsync("type/all");
          
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                List<TypeReadDTO>? types = await response.Content.ReadFromJsonAsync<List<TypeReadDTO>>();
                
                Assert.NotNull(types);                              
                Assert.Contains(types, t => t.Name == "TestType");
            }
        }

        [Fact]
        public async Task Delete_Type_Returns_Expected()
        {
            TypeCreateDTO input = new TypeCreateDTO
            {
                Name = "TestTypeToDelete",
            };

            HttpClient adminClient = _factory.CreateAdminClient();
            HttpResponseMessage newType = await adminClient.PostAsJsonAsync("type/add", input);

            Assert.Equal(HttpStatusCode.OK, newType.StatusCode);

            int newTypeId = await newType.Content.ReadFromJsonAsync<int>();
            HttpResponseMessage response = await adminClient.DeleteAsync($"type/delete/{newTypeId}");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }      
    }
}
