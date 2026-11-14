using Common.DTOs.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using WebAPI.Context;
using WebAPI.Models;
using WebAPI.Security;

namespace WebAPI.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{   
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseSetting("Jwt:SecureKey", "C7SycS4BHrcfrsZII1irOL04Q4A5e0J3n5jZKvLjMK79j+SQ4w3EXX3+IPXR86lS");
        builder.UseSetting("Jwt:Issuer", "test-issuer");
        builder.UseSetting("Jwt:Audience", "test-audience");

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();                    
            logging.AddConsole();                        
            logging.SetMinimumLevel(LogLevel.Warning);   
        });

        var databaseName = "TestDb_" + Guid.NewGuid();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AccessibilityDbContext>>();

            services.AddDbContext<AccessibilityDbContext>(options =>
                options.UseInMemoryDatabase(databaseName));
        });     
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var testHost = base.CreateHost(builder);

        using (var scope = testHost.Services.CreateScope())
        {           
            var db = scope.ServiceProvider.GetRequiredService<AccessibilityDbContext>();
            
            db.Database.EnsureCreated();

            if(!db.Accessibilities.Any())
            {
                db.Accessibilities.AddRange(
                    new Accessibility { Id = 1, Name = "Wheelchair access" },
                    new Accessibility { Id = 2, Name = "Sign language" },
                    new Accessibility { Id = 3, Name = "Braille" }
                );
            }


            if(!db.Types.Any())
            {
                db.Types.AddRange(
                    new Models.Type { Id = 1, Name = "Sport" },
                    new Models.Type { Id = 2, Name = "Culture" }
                );
            }

            if(!db.Logs.Any())
            {
                db.Logs.AddRange(
                    new Log { Id = 1, Timestamp = DateTime.UtcNow, Message = "Test log 1" },
                    new Log { Id = 2, Timestamp = DateTime.UtcNow, Message = "Test log 2" }
                );
            }

            if(!db.Users.Any())
            {
                var adminSalt = PasswordHashProvider.GetSalt();
                var userSalt = PasswordHashProvider.GetSalt();

                db.Users.AddRange(               
                    new User
                    { 
                        Username = "admin", 
                        PasswordSalt = adminSalt,
                        PasswordHash = PasswordHashProvider.GetHash("admin123", adminSalt),                                            
                        FirstName = "admin",
                        LastName = "admin",
                        Email = "admin@example.com"
                    },
                        
                    new User
                    {
                        Username = "user",
                        PasswordSalt = userSalt,
                        PasswordHash = PasswordHashProvider.GetHash("user123", userSalt),                        
                        FirstName = "user",
                        LastName = "user",
                        Email = "user@example.com"
                    }
                );
            }

            if(!db.Activities.Any())
            {
                db.Activities.AddRange(
                    new Activity
                    {
                        Id = 1,
                        Name = "Wheelchair basketball",
                        Description = "Weekly training for all levels",
                        Address = "Ilica 1, Zagreb",
                        Contact = "+385 1 111 1111",
                        Email = "basketball@test.com",
                        TypeId = 1,
                        Views = 10
                    },
                    new Activity
                    {
                        Id = 2,
                        Name = "Museum tour",
                        Description = "Guided tour with sign language",
                        Address = "Trg 2, Zagreb",
                        Contact = "+385 1 222 2222",
                        Email = "museum@test.com",
                        TypeId = 2,
                        Views = 5
                    }
                );
            }

            if(!db.Reviews.Any())
            {               
                db.Reviews.AddRange(
                    new Review { Id = 1, Comment = "Great place!", Grade = 5, Date = DateTime.UtcNow, UserId = 2, ActivityId = 1 },
                    new Review { Id = 2, Comment = "Good, but crowded", Grade = 3, Date = DateTime.UtcNow, UserId = 2, ActivityId = 2 }
                );
            }

            if(!db.Accessibilities.Any())
            {
                db.ActivityAccessibilities.AddRange(
                    new ActivityAccessibility { ActivityId = 1, AccessibilityId = 1 },
                    new ActivityAccessibility { ActivityId = 2, AccessibilityId = 1 },
                    new ActivityAccessibility { ActivityId = 2, AccessibilityId = 2 }
                );
            }

            db.SaveChanges();
        }

        return testHost;
    }

    public static string CreateTestToken(string username, int userId, string role = "User")
    {        
        JwtSecurityTokenHandler tokenHandler = new();

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C7SycS4BHrcfrsZII1irOL04Q4A5e0J3n5jZKvLjMK79j+SQ4w3EXX3+IPXR86lS")),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        tokenDescriptor.Subject = new ClaimsIdentity(
            new List<Claim>
            {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
            }
        );

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}