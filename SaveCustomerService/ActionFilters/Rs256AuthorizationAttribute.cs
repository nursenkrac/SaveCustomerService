using Core.Model.Infrastructure.Results;
using SaveCustomerService.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace SaveCustomerService.ActionFilters
{
    public class Rs256AuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAuthorized = false;
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    if (!IsValid(token))
                        isAuthorized = false;
                    else
                        isAuthorized = true;
                }
                else
                    isAuthorized = false;
            }
            else
                isAuthorized = false;
            if (!isAuthorized)
            {
                context.Result = new ObjectResult(context.ModelState)
                {
                    Value = new ResultModel() { IsSuccess = false, Message = "Authorization has been denied for this request." },
                    StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized
                };
                return;
            }
            base.OnActionExecuting(context);
        }
        bool IsValid(string token)
        {
            var configurationRoot = IocManager.Resolve<IConfigurationRoot>();
            var _publicKey = Convert.FromBase64String(configurationRoot.GetSection("Jwt:RsaPublicKey").Value);
            using RSA rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(_publicKey, out _);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                CryptoProviderFactory = new CryptoProviderFactory()
                {
                    CacheSignatureProviders = false
                }
            };
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, validationParameters, out var validatedSecurityToken);
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
    }
}
