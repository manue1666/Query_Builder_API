using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QueryBuilderApi.Models;

namespace QueryBuilderApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {

            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("JWT Secret Key not configured");


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            
            var token = new JwtSecurityToken(
                issuer: "QueryBuilderAPI",
                audience: "QueryBuilderClients",
                claims: new[] {
                    new System.Security.Claims.Claim("sub", user.Id.ToString()),
                    new System.Security.Claims.Claim("email", user.Email),
                    new System.Security.Claims.Claim("name", user.Name)
                },
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

           
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
        
    }
}