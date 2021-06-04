using EWorksPromotionCampaign.Service.Security;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Util
{
    public class TwoFactorAuthentication : ITwoFactorAuthentication
    {
        private readonly TokenType _tokenType;
        private TwoFactorAuthentication(TokenType tokenType)
        {
            _tokenType = tokenType;
        }
        public TwoFactorAuthentication() { }
        public (string, string, string) GenerateToken()
        {
            return _tokenType switch
            {
                TokenType.otp => GenerateOtp(),
                TokenType.jwt => GenerateJwt(),
                TokenType.hashed => GenerateHashCode(),
                _ => ("", "", ""),
            };
        }

        private static (string, string, string) GenerateJwt()
        {
            var tokenData = new JwtSecurityToken(expires: DateTime.UtcNow.AddMinutes(20));
            var token = new JwtSecurityTokenHandler().WriteToken(tokenData);
            return (token, "", "");
        }
        private static (string, string, string) GenerateOtp()
        {
            var salt = PasswordSalt.Create();
            var otp = Helper.RandomString(6);
            var token = PasswordHash.Create(otp, salt);
            return (token, salt, otp);
        }

        private static (string, string, string) GenerateHashCode()
        {
            var otp = Helper.RandomString(6);
            var token = HashCode(otp);
            return (token, "", otp);
        }

        public static TwoFactorAuthentication Type(TokenType type)
        {
            return new TwoFactorAuthentication(type);
        }
        public static string HashCode(string input)
        {
            string code;
            using (SHA1Managed sha1 = new())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var temp = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    temp.Append(b.ToString("X2"));
                }
                code = temp.ToString();
            }
            return code;
        }
        public JwtSecurityToken ExtractTokenData(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
        public string Requester { get; set; }
        public string Token { get; set; }
        public string Salt { get; set; }
        public string TypeOfToken { get; set; }
        public string RequestId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
