using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Shared.Util
{
    public interface ITwoFactorAuthentication
    {
        string Requester { get; set; }
        string Token { get; set; }
        string Salt { get; set; }
        string TypeOfToken { get; set; }
        string RequestId { get; set; }
        DateTime CreatedAt { get; set; }
        (string, string, string) GenerateToken(int length, DigitType digitType = DigitType.alphanumeric);
        JwtSecurityToken ExtractTokenData(string token);
    }
    public class TwoFactorAuthenticationDto : ITwoFactorAuthentication
    {
        public string Requester { get; set; }
        public string Token { get; set; }
        public string Salt { get; set; }
        public string TypeOfToken { get; set; }
        public string RequestId { get; set; }
        public DateTime CreatedAt { get; set; }

        public JwtSecurityToken ExtractTokenData(string token)
        {
            throw new NotImplementedException();
        }
        public (string, string, string) GenerateToken(int length, DigitType digitType = DigitType.alphanumeric)
        {
            return ("", "", ""); 
        }
    }
}
