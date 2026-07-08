using WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Security
{
    public class JwtTokenProvider
    {
        public static string CreateToken(string subjectName, int subjectId, string role, string secureKey, int expiration)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(expiration),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            tokenDescriptor.Subject = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, subjectId.ToString()),
                    new Claim(ClaimTypes.Name, subjectName.ToString()),
                    new Claim(ClaimTypes.Role, (role == "true") ? "Admin" : "User")
                }
            );
                     
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);            

            return tokenHandler.WriteToken(token);
        }
    }
}
