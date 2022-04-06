using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MPara.Authentication.Models;
using MPara.Repositories.Abstract;
using MPara.Repositories.Entity;

namespace MPara.Authentication.Helper
{
    public class TokenHelper
    {
        private IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;

        public TokenHelper(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public AppUser AuthenticateUser(AppUserModel login)
        {
            var encryptedPass = Encrypt(login.Password);
            return _unitOfWork.AppUserRepository.Find(x => x.Username == login.Username
                                                        && x.Password == encryptedPass)
                                                .FirstOrDefault();
        }

        public string Encrypt(string toBeEncrypted)
        {
            using (SHA256 hash = SHA256.Create())
            {
                return String.Concat(hash
                  .ComputeHash(Encoding.UTF8.GetBytes(toBeEncrypted))
                  .Select(item => item.ToString("x2")));
            }
        }

        public Token CreateAccessToken(AppUser userInfo)
        {
            Token tokenInstance = new Token();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[] {
                new Claim("Username", userInfo.Username),
                new Claim("AppUserId", userInfo.AppUserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            tokenInstance.Expiration = DateTime.Now.AddMinutes(10);
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims,
                expires: tokenInstance.Expiration,
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            tokenInstance.AccessToken = tokenHandler.WriteToken(securityToken);
            tokenInstance.RefreshToken = CreateRefreshToken();
            return tokenInstance;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }

    }
}
