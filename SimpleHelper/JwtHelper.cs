using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace SimpleHelper
{
    public static class JwtHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="securityKey"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(string key, string securityKey, string issuer, string audience, int duration)
        {
            var symmetricKey = Convert.FromBase64String(securityKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, key)
                }),
                Audience = audience,
                Issuer = issuer,
                Expires = DateTime.UtcNow.AddMinutes(duration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(stoken);
        }
    }
}
