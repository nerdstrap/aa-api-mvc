using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Nerdstrap.Identity.IdentityManagerWeb.Security
{
    public static class TokenExtensions
    {
        public static string CreateToken(string userId, string accessLevel)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim("accesslevel", accessLevel)
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            var issuedAt = DateTime.UtcNow;
            var expires = issuedAt.AddMinutes(SecurityConfiguration.TokenLifetimeInMinutes);

            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = SecurityConfiguration.TokenIssuer,
                Audience = SecurityConfiguration.TokenAudience,
                SigningCredentials = SecurityConfiguration.SigningCredentials,
                IssuedAt = issuedAt,
                Expires = expires
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public static ClaimsPrincipal ValidateToken(string token)
        {
            ClaimsPrincipal claimsPrincipal = null;

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = SecurityConfiguration.TokenIssuer,
                ValidateAudience = true,
                ValidAudience = SecurityConfiguration.TokenAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecurityConfiguration.SecurityKey,
                RequireExpirationTime = true,
                ValidateLifetime = true
            };
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.InboundClaimTypeMap["name"] = ClaimTypes.Name;

                SecurityToken securityToken;
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParams, out securityToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return claimsPrincipal;
        }
    }
}
