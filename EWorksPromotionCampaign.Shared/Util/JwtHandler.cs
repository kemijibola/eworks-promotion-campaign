using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Shared.Util
{
    public interface ITokenHandler
    {
        string GenerateToken(GenerateTokenRequest request, bool isRefresh = false);
    }
    public class JwtHandler : ITokenHandler
    {
        private readonly AppSettings _appSettings;
        public JwtHandler(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string GenerateToken(GenerateTokenRequest request, bool isRefresh)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, request.ClaimTypes[IdentityClaimType.name]),
                    new Claim(ClaimTypes.Role, request.ClaimTypes[IdentityClaimType.role])
                }),
                Expires = isRefresh ? DateTime.UtcNow.AddDays(1) : DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
