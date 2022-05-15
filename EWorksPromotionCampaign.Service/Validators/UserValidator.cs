using EWorksPromotionCampaign.Shared.Models.Input;
using EWorksPromotionCampaign.Shared.Models.Input.Account;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Validators
{
    public interface IUserValidator
    {
        ValidationResult ValidateNewUser(RegisterInputModel newUser);
    }
    public class UserValidator : IUserValidator
    {
        public ValidationResult ValidateNewUser(RegisterInputModel newUser)
        {
            _ = newUser ?? throw new ArgumentNullException(nameof(newUser), "User is required");

            var result = new ValidationResult();
            var regex = new Regex(Constants.PhoneNumberRegexFormat);
            if (newUser.Phone.Length != 13 || !regex.IsMatch(newUser.Phone))
                result.Errors.Add(nameof(newUser.Phone), "Enter a valid mobile number format starting with 234.");
            return result;
        }
    }
}
