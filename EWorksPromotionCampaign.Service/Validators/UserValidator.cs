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
        ValidationResult ValidateForgotPassword(ForgotPasswordInputModel forgotPassword);
        ValidationResult ValidateResetPassword(ResetPasswordInputModel resetPassword);
    }
    public class UserValidator : IUserValidator
    {
        public ValidationResult ValidateNewUser(RegisterInputModel newUser)
        {
            _ = newUser ?? throw new ArgumentNullException(nameof(newUser), "User is required");

            var result = new ValidationResult();
            if (string.IsNullOrEmpty(newUser.FirstName))
                result.Errors.Add(nameof(newUser.FirstName), "FirstName is required.");
            if (string.IsNullOrWhiteSpace(newUser.Password))
                result.Errors.Add(nameof(newUser.Password), "Password is required.");
            if (!newUser.Password.Equals(newUser.ConfirmPassword))
                result.Errors.Add(nameof(newUser.ConfirmPassword), "Password and Confirm Password must match.");
            if (newUser.Password.Length < 8)
                result.Errors.Add(nameof(newUser.Password), "Password length must be greater than 7.");
            if (string.IsNullOrEmpty(newUser.LastName))
                result.Errors.Add(nameof(newUser.LastName), "LastName is required.");
            if (string.IsNullOrEmpty(newUser.Phone))
                result.Errors.Add(nameof(newUser.Phone), "Phone is required.");
            var regex = new Regex(Constants.PhoneNumberRegexFormat);
            if (newUser.Phone.Length != 13 || !regex.IsMatch(newUser.Phone))
                result.Errors.Add(nameof(newUser.Phone), "Enter a Valid mobile number format starting with 234.");
            if (string.IsNullOrEmpty(newUser.Email))
                result.Errors.Add(nameof(newUser.Email), "Email is required.");
            return result;
        }
        public ValidationResult ValidateResetPassword(ResetPasswordInputModel resetPassword)
        {
            _ = resetPassword ?? throw new ArgumentNullException(nameof(resetPassword), "Reset Password data is required");
            var result = new ValidationResult();
            if (string.IsNullOrEmpty(resetPassword.Email))
                result.Errors.Add(nameof(resetPassword.Email), "Email is required.");
            if (string.IsNullOrWhiteSpace(resetPassword.Password))
                result.Errors.Add(nameof(resetPassword.Password), "Password is required.");
            if (!resetPassword.Password.Equals(resetPassword.ConfirmPassword))
                result.Errors.Add(nameof(resetPassword.ConfirmPassword), "Password and Confirm Password must match.");
            if (string.IsNullOrWhiteSpace(resetPassword.Token))
                result.Errors.Add(nameof(resetPassword.Token), "Token is required.");
            return result;
        }
        public ValidationResult ValidateForgotPassword(ForgotPasswordInputModel forgotPassword)
        {
            var result = new ValidationResult();
            if (string.IsNullOrEmpty(forgotPassword.Email))
                result.Errors.Add(nameof(forgotPassword.Email), "Email is required.");
            return result;
        }
    }
}
