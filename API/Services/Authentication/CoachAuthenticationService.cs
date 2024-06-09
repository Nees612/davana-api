using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Services.Authentication.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services.Authentication
{
    public class CoachAuthenticationService : IAuthenticationService<Coach>
    {
        private readonly IConfigurationRoot _conf;
        public CoachAuthenticationService()
        {
            _conf = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
        }

        public async Task<string> GenerateToken(Coach entity)
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

        private ClaimsIdentity GenerateClaims(Coach entity)
        {
            _ = entity.EmailAddress ?? throw new ArgumentException("Parameter cannot be null", nameof(entity.EmailAddress));
            _ = entity.Scopes ?? throw new ArgumentException("Parameter cannot be null", nameof(entity.Scopes));
            _ = entity.Roles ?? throw new ArgumentException("Parameter cannot be null", nameof(entity.Roles));

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, entity.EmailAddress));
            claims.AddClaim(new Claim("sub", entity.EmailAddress));
            claims.AddClaim(new Claim("jti", DateTime.Now.ToBinary().ToString() + entity.Id));

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
    }
}