using NexumTech.Infra.API.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using NexumTech.Infra.Models;

namespace NexumTech.Infra.API
{
    public class TokenService : ITokenService
    {
        private readonly AppSettingsAPI _appSettingsAPI;
        public TokenService(IOptions<AppSettingsAPI> appSettingsAPI)
        {
            _appSettingsAPI = appSettingsAPI.Value;
        }

        public string GenerateToken(UserViewModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettingsAPI.JWT.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
