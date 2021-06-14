using EWorksPromotionCampaign.Shared.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWorksPromotionCampaign.Shared.Models.Domain;

namespace EWorksPromotionCampaign.Shared.Models.Output
{
    public class RegisterOutputModel
    {
        private RegisterOutputModel(User user, string token = "")
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
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public string Token { get; set; }

        public static RegisterOutputModel FromUser(User user, string token = "")
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            return new RegisterOutputModel(user, token);
        }
    }
}
