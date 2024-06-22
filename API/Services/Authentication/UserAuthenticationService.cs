using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.Services.Authentication.Interfaces;
using API.Services.Authentication.Models;
using Microsoft.IdentityModel.Tokens;

namespace API.Services.Authentication
{
    public static class UserAuthenticationService
    {
        private static readonly IConfigurationRoot _conf = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
        public static async Task<string> GenerateToken(User entity)
        {
            _ = entity ?? throw new ArgumentException("Parameter cannot be null", nameof(entity));

            return await Task.Run(() =>
            {
                var handler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
                var credentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = GenerateClaims(entity),
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = credentials,
                    Issuer = _conf.GetSection("Authentication:Schemes:Bearer:ValidIssuer").Get<string>()
                };

                var token = handler.CreateToken(tokenDescriptor);
                return handler.WriteToken(token);
            });
        }

        private static ClaimsIdentity GenerateClaims(User entity)
        {
            _ = entity.EmailAddress ?? throw new ArgumentException("Parameter cannot be null", nameof(entity.EmailAddress));
            _ = entity.Scopes ?? throw new ArgumentException("Parameter cannot be null", nameof(entity.Scopes));
            _ = entity.Roles ?? throw new ArgumentException("Parameter cannot be null", nameof(entity.Roles));
            _ = entity.FirstName ?? throw new ArgumentException("Parameter cannot be null", nameof(entity.FirstName));
            _ = entity.LastName ?? throw new ArgumentException("Parameter cannot be null", nameof(entity.LastName));
            _ = entity.Id ?? throw new ArgumentException("Parameter cannot be null", nameof(entity.Id));

            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.Name, entity.EmailAddress));
            claims.AddClaim(new Claim("sub", entity.EmailAddress));
            claims.AddClaim(new Claim("name", $"{entity.FirstName} {entity.LastName}"));
            claims.AddClaim(new Claim("jti", DateTime.Now.ToBinary().ToString() + entity.Id));
            claims.AddClaim(new Claim("funny", entity.Id.ToString()));

            foreach (var scope in entity.Scopes)
                claims.AddClaim(new Claim("scope", scope));
            foreach (var role in entity.Roles)
                claims.AddClaim(new Claim(ClaimTypes.Role, role));

            var ValidAudiences = _conf.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<List<string>>();

            if (ValidAudiences != null)
                foreach (var aud in ValidAudiences)
                    claims.AddClaim(new Claim("aud", aud));

            return claims;
        }

        public static async Task<JwtSecurityToken?> DecodeToken(string encodedJWT)
        {
            _ = encodedJWT ?? throw new ArgumentException("Parameter cannot be null", nameof(encodedJWT));

            return await Task.Run(() =>
            {
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken;

                try
                {
                    jwtSecurityToken = handler.ReadJwtToken(encodedJWT);
                }
                catch (Exception ex)
                {
                    //Log the erorr;
                    return null;
                }
                return jwtSecurityToken;
            });
        }
    }
}