using EWorksPromotionCampaign.Shared.Models.Input;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Validators
{
    public interface ILoginValidator
    {
        ValidationResult ValidateLogin(LoginInputModel login);
    }
    public class LoginValidator : ILoginValidator
    {
        public ValidationResult ValidateLogin(LoginInputModel login)
        {
            _ = login ?? throw new ArgumentNullException(nameof(login), "Login is required");

            var result = new ValidationResult();
            if (string.IsNullOrEmpty(login.Email) && string.IsNullOrEmpty(login.Phone))
                result.Errors.Add($"{nameof(login.Email)}/{nameof(login.Phone)}", "Email/Phone is required.");

            if (!string.IsNullOrEmpty(login.Phone))
            {
                var regex = new Regex(Constants.PhoneNumberRegexFormat);
                if (login.Phone.Length != 13 || !regex.IsMatch(login.Phone))
                    result.Errors.Add(nameof(login.Phone), "Enter a Valid mobile number format starting with 234.");
            }

            return result;
        }
    }
}
