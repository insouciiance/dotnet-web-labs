using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinanceManagerServices.Authentication
{
    public class JwtTokenHelper
    {
        public const string SECRET_PHRASE_KEY = "SigningKeyPhrase";

        public const string AUTH_POLICY_NAME = "Auth";

        public const int AUTH_TOKEN_LIFETIME_MINUTES = 60;

        private readonly IConfiguration _configuration;

        public JwtTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime Expires) CreateAuthToken(IdentityUser user)
        {
            Claim[] claims =
            {
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (JwtRegisteredClaimNames.Typ, AUTH_POLICY_NAME)
            };

            string signingKeyPhrase = _configuration[SECRET_PHRASE_KEY];
            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(signingKeyPhrase));
            SigningCredentials signingCredentials = new(signingKey, SecurityAlgorithms.HmacSha256);

            DateTime expires = DateTime.Now.AddMinutes(AUTH_TOKEN_LIFETIME_MINUTES);

            JwtSecurityToken jwt = new(
                signingCredentials: signingCredentials,
                claims: claims,
                expires: expires);

            return (new JwtSecurityTokenHandler().WriteToken(jwt), expires);
        }
    }
}