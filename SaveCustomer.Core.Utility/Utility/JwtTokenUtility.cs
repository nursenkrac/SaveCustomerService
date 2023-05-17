using SaveCustomerService.Core.Infrastructure;
using SaveCustomerService.Model.DtoModel.User.Dto;
using SaveCustomerService.Model.ViewModel.User.Outputs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SaveCustomerService.Core.Utility.Utility
{
    public class JwtTokenUtility
    {
        private readonly UserInfo _userDto;
        private readonly string _privateKey;

        public JwtTokenUtility(UserInfo userDto)
        {
            var configurationRoot = IocManager.Resolve<IConfigurationRoot>();
            _userDto = userDto;
            _privateKey = configurationRoot.GetSection("Jwt:RsaPrivateKey").Value;
        }

        class CustomClaims
        {
            public int UserId { get; set; }
            public string UserInfo { get; set; }
            public string IdentityNo { get; set; }
            public string Maintenance { get; set; }
        }

        public class UserInfoOutputModel
        {
            public string Identitiy { get; set; }
            public string Phone { get; set; }
            public string FullName { get; set; }

        }

        private string GenerateToken(DateTime expireDate)
        {
            var privateKey = Convert.FromBase64String(_privateKey);
            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKey, out _);
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };
            var now = DateTime.Now;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email,_userDto.Identitiy),
                new Claim(nameof(CustomClaims.UserInfo), JsonConvert.SerializeObject(GetUserInfo())),

            };
            JwtSecurityToken jwt = new JwtSecurityToken(
                claims: claims,
                expires: expireDate,
                signingCredentials: signingCredentials
            );
            return jwt != null ? new JwtSecurityTokenHandler().WriteToken(jwt) : string.Empty;
        }

        public UserTokenOutputModel CreateToken()
        {
            DateTime expireDate = DateTime.UtcNow.AddDays(180);
            var token = GenerateToken(expireDate);
            if (!string.IsNullOrEmpty(token))
                return new UserTokenOutputModel
                {
                    Token = token,
                    ExpireDate = expireDate
                };
            return null;
        }

        public UserTokenOutputModel CreateForgotPasswordToken()
        {
            DateTime expireDate = DateTime.Now.AddMinutes(10);
            var token = GenerateToken(expireDate);
            if (!string.IsNullOrEmpty(token))
                return new UserTokenOutputModel
                {
                    Token = token,
                    ExpireDate = expireDate
                };
            return null;
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

        public UserInfoOutputModel GetUserInfo()
        {
            return new UserInfoOutputModel() { Identitiy = _userDto.Identitiy };
        }
    }
}
