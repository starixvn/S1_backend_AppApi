using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MobileApiServer.Models;

namespace MobileApiServer.Helpers
{
    public class JwtHelpers
    {
        private readonly IConfiguration _configuration;

        public JwtHelpers(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generate JWT Token
        /// </summary>
        /// <param name="customer">customer info</param>
        /// <param name="token">token saved in OnUser</param>
        /// <param name="expireMinutes">jwt token expire in (default 30)min</param>
        /// <returns></returns>
        public TokenResponse GenerateToken(Customer customer, string token, int expireMinutes = 30)
        {
            var issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
            var signKey = _configuration.GetValue<string>("JwtSettings:SignKey");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.UserName),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(expireMinutes).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("CustId", customer.CustId.ToString()),
                new Claim("Token", token),
                new Claim("SiteId",customer.SiteId.ToString() ),
                new Claim("LangId",customer.UserLang )
            };

            var userClaimsIdentity = new ClaimsIdentity(claims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // get SecurityTokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Subject = userClaimsIdentity,
                Expires = DateTime.Now.AddMinutes(expireMinutes),
                SigningCredentials = signingCredentials
            };

            // gen JWT securityToken 
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return new TokenResponse
            {
                AccessToken = serializeToken,
                ExpiresIn = (long)TimeSpan.FromMinutes(expireMinutes).TotalSeconds - 1
            };
        }

        public class TokenResponse
        {
            public string AccessToken { get; set; }
            public string TokenType => "bearer";
            public long ExpiresIn { get; set; }
        }
    }
}
