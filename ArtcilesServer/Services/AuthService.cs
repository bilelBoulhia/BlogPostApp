
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using ArtcilesServer.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;

namespace ArticlesServer.Services
{
    public class AuthService
    {
        private readonly string _key;
        private readonly string _time;
        private readonly string _issuer;
        private readonly string _audience;
        public AuthService(IConfiguration config)
        {
            _key = config["Jwt:Key"];
            _time = config["Jwt:TokenExpiryInMinutes"];
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
        }

        public string GenerateToken(User user)
        {

            var claims = new Dictionary<string, object>
            {
                [ClaimTypes.Name] = user.UserName,
                [ClaimTypes.NameIdentifier] = user.UserId.ToString(),
                [ClaimTypes.Email] = user.UserEmail
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creditianls = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = claims,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_time)),
                SigningCredentials = creditianls,
                Issuer = _issuer,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.UtcNow,
                Audience = _audience
            };
            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_key);

            try {

                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token,new TokenValidationParameters

                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)

                }, out SecurityToken validatedToken);

                return claimsPrincipal;
            }
            catch {
                throw new ApplicationException("Token has expired.");
            }
        }






    }


   


}