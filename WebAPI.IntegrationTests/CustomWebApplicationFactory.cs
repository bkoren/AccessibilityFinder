using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using WebAPI.Context;
using WebAPI.Security;

namespace WebAPI.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestJwtKey =
        "C7SycS4BHrcfrsZII1irOL04Q4A5e0J3n5jZKvLjMK79j+SQ4w3EXX3+IPXR86lS";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Jwt:SecureKey", TestJwtKey);

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();                    
            logging.AddConsole();                        
            logging.SetMinimumLevel(LogLevel.Warning);   
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AccessibilityDbContext>>();

            services.AddDbContext<AccessibilityDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        });
    }

    public string GetTestJwtToken(string subjectName, int subjectId, string role, int expiration)
    {
        return JwtTokenProvider.CreateToken(subjectName, subjectId, role, TestJwtKey, expiration);
    }
    public HttpClient CreateAdminClient()
    {
        HttpClient client = CreateClient();

        string token = GetTestJwtToken("TestAdmin", 1, "true", 60);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return client;
    }
}