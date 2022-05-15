using EWorksPromotionCampaign.Shared.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Output
{
    public class LoginOutputModel : BaseUserOutputModel
    {
        private LoginOutputModel(User user, string token = "", string refreshToken = "")
        {
            Id = user.Id;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            Phone = user.Phone;
            Email = user.Email;
            Address = user.Address;
            DateOfBirth = user.DateOfBirth;
            IsPhoneVerified = user.IsPhoneVerified;
            IsEmailVerified = user.IsEmailVerified;
            Token = token;
        }
        public string Token { get; set; }

        public static LoginOutputModel FromUser(User user, string token = "", string refreshToken = "")
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            return new LoginOutputModel(user, token, refreshToken);
        }
    }
}
