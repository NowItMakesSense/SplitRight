using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SplitRight.Domain.Commom;
using SplitRight.Domain.Contracts.Entities;
using SplitRight.Domain.Contracts.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SplitRight.Domain.Contracts.Services
{
    public sealed class JwtTokenGeneratorService : IJwtTokenGeneratorService
    {
        private readonly JwtSettings _settings;

        public JwtTokenGeneratorService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateAccessToken(User user, Guid sessionId)
        {
            var claims = BuildClaims(user, sessionId);
            var credentials = GetSigningCredentials();

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        private List<Claim> BuildClaims(User user, Guid sessionId)
        {
            return
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("sessionId", sessionId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));

            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }
}
