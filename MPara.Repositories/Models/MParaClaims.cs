using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MPara.Repositories.Models
{

    public static class MParaClaims
    {
        public static ClaimModel GetClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jwtSecurityToken == null)
                return new ClaimModel();

            return new ClaimModel
            {
                Username = jwtSecurityToken.Claims.First(claim => claim.Type == "Username").Value,
                ApiUserId = int.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "AppUserId").Value)
            };
        }
    }
}
