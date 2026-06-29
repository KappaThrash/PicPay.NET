using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PicPay.Domains;

namespace PicPay.Services.Auth
{
    public class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
    {
        private readonly JwtOptions _options = options.Value;

        public AuthTokenDTO GenerateToken(Usuario usuario, IEnumerable<string>? roles = null)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, usuario.Email ?? string.Empty),
                new(ClaimTypes.Name, usuario.Nome ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles ?? Enumerable.Empty<string>())
            {
                claims.Add(new Claim(_options.RoleClaimType, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new AuthTokenDTO
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = "Bearer",
                ExpiresAt = expiresAt
            };
        }
    }
}
