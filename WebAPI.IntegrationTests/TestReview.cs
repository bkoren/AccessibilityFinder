using Azure;
using Common.DTOs.Activity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Context;

namespace WebAPI.IntegrationTests
{
    public class TestReview : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        private string userToken;

        public TestReview(CustomWebApplicationFactory testFactory)
        {
            _client = testFactory.CreateClient();

            userToken = CustomWebApplicationFactory.CreateTestToken("user", 2);
        }

        [Fact]
        public async Task TestGetReviews() {

            int articleId = 1;

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            var response = await _client.GetAsync("/review/get/" + articleId);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestAddReview() {

            var review = new
            {
                activityId = 1,
                comment = "Test",
                grade = 5
            };

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            var response = await _client.PostAsJsonAsync("/review/add", review);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);         

            var responseCheck = await _client.GetAsync("/activity/get/" + 1);

            ActivityGetDTO? activity = await responseCheck.Content.ReadFromJsonAsync<ActivityGetDTO>();

            Assert.NotNull(activity);

            Assert.True(activity.Reviews.Exists(r => r.Comment == "Test"));
        }
    }
}
