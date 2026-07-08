using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace WebAPI.Security
{
    public class PasswordHashProvider
    {
        public static string GetSalt() 
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));         

        public static string GetHash(string password, string b64salt)
        {
            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Convert.FromBase64String(b64salt),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 32
                );

            return Convert.ToBase64String(hash);
        }
    }
}
