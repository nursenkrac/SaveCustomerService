using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SaveCustomerService.Presentation.Api.Helpers
{
    public class TokenUser
    {
        public TokenUser(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                string token = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                string lang = httpContextAccessor.HttpContext.Request.Headers["Language"];
                Language = lang ?? "tr";
                if (!string.IsNullOrEmpty(token))
                {
                    token = token.Replace("Bearer ", "");
                    var handler = new JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        JwtSecurityToken jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                        Claim userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                        if (userIdClaim != null)
                            UserId = int.Parse(userIdClaim.Value);

                        Claim identityClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "IdentityNo");
                        if (identityClaim != null)
                            IdentityNo = identityClaim.Value;

                        Claim maintenanceClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Maintenance");
                        if (maintenanceClaim != null)
                            Maintenance = int.Parse(maintenanceClaim.Value);
                    }
                }
            }
            catch (Exception)
            {

            }

        }
        public int UserId { get; set; } = 0;
        public string IdentityNo { get; set; }
        public int Maintenance { get; set; }
        public string Language { get; set; }
    }
}
