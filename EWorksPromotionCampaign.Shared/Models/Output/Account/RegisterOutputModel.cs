using EWorksPromotionCampaign.Shared.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWorksPromotionCampaign.Shared.Models.Domain;

namespace EWorksPromotionCampaign.Shared.Models.Output
{
    public class RegisterOutputModel : BaseUserOutputModel
    {
        private RegisterOutputModel(User user, string token = "")
        {
            Id = user.Id;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            Phone = user.Phone;
            Address = user.Address;
            DateOfBirth = user.DateOfBirth;
            IsPhoneVerified = user.IsPhoneVerified;
            IsEmailVerified = user.IsEmailVerified;
            Token = token;
        }
        public string Token { get; set; }

        public static RegisterOutputModel FromUser(User user, string token = "")
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            return new RegisterOutputModel(user, token);
        }
    }
}
