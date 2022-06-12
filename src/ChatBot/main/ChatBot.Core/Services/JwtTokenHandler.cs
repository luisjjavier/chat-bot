using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace ChatBot.Core.Services
{
    public sealed class JwtTokenHandler: ITokenHandler
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenHandler(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;

        }
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private IList<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String),
                new Claim("userName", user.UserName, ClaimValueTypes.String)
            };
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IList<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.ExpiryInDays),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        public string GenerateToken(User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
    }
}
