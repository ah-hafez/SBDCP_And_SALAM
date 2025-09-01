using MobileApi.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MobileApi.Common
{
    public class AuthenticationModule
    {
        private static readonly string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

        SecurityKey signingKey =
               new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

        // The Method is used to generate token for user
        public string GenerateTokenForUser(string userName, int userId)
        {
            DateTime now = DateTime.UtcNow;
            SigningCredentials signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            }, "Custom");

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = "http://www.sssit.net",
                Issuer = "self",
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                Expires = now.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["TokenTimeoutInMinutes"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            return signedAndEncodedToken;
        }

        /// Using the same key used for signing token, user payload is generated back
        public JwtSecurityToken GenerateUserClaimFromJWT(string authToken)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                      {
                    "http://www.sssit.net",
                      },

                ValidIssuers = new string[]
                  {
                      "self",
                  },
                IssuerSigningKey = signingKey
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(authToken, tokenValidationParameters, out validatedToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return validatedToken as JwtSecurityToken;
        }

        public AuthenticationIdentity PopulateUserIdentity(JwtSecurityToken userPayloadToken)
        {
            string name = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "unique_name").Value;
            string userId = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "nameid").Value;

            return new AuthenticationIdentity(name) { UserId = Convert.ToInt32(userId), UserName = name };
        }
    }

}