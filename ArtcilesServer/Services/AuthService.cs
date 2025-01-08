
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using ArtcilesServer.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace ArticlesServer.Services
{
    public class AuthService
    {
        private readonly string _key;
        private readonly string _time;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly TokenValidationParameters _validationParameters;
        public AuthService(IConfiguration config)
        {
            _key = config["Jwt:Key"];
            _time = config["Jwt:TokenExpiryInMinutes"];
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
            _validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
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

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        public string GenerateNewAccessToken(TokenRequest tokenRequest)
        {
        
      ;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

       
                ClaimsPrincipal principal = tokenHandler.ValidateToken(
                    tokenRequest.token,
                    _validationParameters,
                    out SecurityToken validatedToken);

                if (!principal.HasClaim(c => c.Type == "tokenType" && c.Value == "refresh"))
                {
                    throw new SecurityTokenException("Invalid token type - refresh token required");
                }

              

                var userId = principal.FindFirst("userId")?.Value;
                var userEmail = principal.FindFirst("email")?.Value;
                var userName = principal.FindFirst("name")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    throw new SecurityTokenException("Required claims are missing");
                }

                var claims = new Dictionary<string, object>
                {
                    [ClaimTypes.NameIdentifier] = userId,
                    [ClaimTypes.Email] = userEmail,
                    [ClaimTypes.Name] = userName,
               
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Claims = claims,
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_time)),
                    SigningCredentials = credentials,
                    Issuer = _issuer,
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow,
                    Audience = _audience
                };

                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(securityToken);
            }
            catch (SecurityTokenException ex)
            {
                throw new SecurityTokenArgumentException("Token validation failed:" + ex);
            }
        }


        public ClaimsPrincipal ValidateToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_key);

            try {

                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token,_validationParameters, out SecurityToken validatedToken);

                return claimsPrincipal;
            }
            catch {
                throw new ApplicationException("Token has expired.");
            }
        }




    }


}


   


