using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Validators.Admin
{
    public interface IAdminUserValidator
    {
        ValidationResult ValidateAdminLogin(AdminLoginInputModel login);
        ValidationResult ValidateNewAdmin(CreateAdminUserInputModel model);
        ValidationResult ValidateUpdateAdmin(UpdateAdminUserInputModel model);
        ValidationResult ValidateFetchAdminByEmail(string email);
        ValidationResult ValidateUpdateUserStatus(UpdateUserStatusInputModel model);
        ValidationResult ValidateUserDisabledStatus(UpdateDisabledStatusInputModel model);
    }
    public class AdminUserValidator : IAdminUserValidator
    {
        public ValidationResult ValidateAdminLogin(AdminLoginInputModel login)
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

        public ValidationResult ValidateFetchAdminByEmail(string email)
        {
            var result = new ValidationResult();
            if (string.IsNullOrEmpty(email))
                result.Errors.Add(nameof(email), "Email is required.");
            return result;

        }

        public ValidationResult ValidateNewAdmin(CreateAdminUserInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "User is required");
            var result = new ValidationResult();
            if (string.IsNullOrEmpty(model.FirstName))
                result.Errors.Add(nameof(model.FirstName), "Provide FirstName.");
            if (string.IsNullOrEmpty(model.LastName))
                result.Errors.Add(nameof(model.LastName), "Provide Lastname.");
            if (string.IsNullOrEmpty(model.Email))
                result.Errors.Add(nameof(model.Email), "Provide Email.");
            if (string.IsNullOrEmpty(model.Phone))
                result.Errors.Add(nameof(model.Phone), "Provide Phone.");
            var regex = new Regex(Constants.PhoneNumberRegexFormat);
            if (model.Phone.Length != 13 || !regex.IsMatch(model.Phone))
                result.Errors.Add(nameof(model.Phone), "Enter a Valid mobile number format starting with 234.");
            if (model.RoleId < 1)
                result.Errors.Add(nameof(model.RoleId), "Provide valid RoleId.");
            return result;

        }

        public ValidationResult ValidateUpdateAdmin(UpdateAdminUserInputModel model)
        {
            var result = new ValidationResult();
            if (!string.IsNullOrEmpty(model.Phone))
            {
                var regex = new Regex(Constants.PhoneNumberRegexFormat);
                if (model.Phone.Length != 13 || !regex.IsMatch(model.Phone))
                    result.Errors.Add(nameof(model.Phone), "Enter a Valid mobile number format starting with 234.");
            }
            return result;
        }

        public ValidationResult ValidateUpdateUserStatus(UpdateUserStatusInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "User is required");
            var result = new ValidationResult();
            if (model.Id < 1)
                result.Errors.Add(nameof(model.Id), "Id is required.");
            if (string.IsNullOrEmpty(model.Comment))
                result.Errors.Add(nameof(model.Comment), "Comment is required.");
            return result;
        }

        public ValidationResult ValidateUserDisabledStatus(UpdateDisabledStatusInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "User is required");
            var result = new ValidationResult();
            if (model.Id < 1)
                result.Errors.Add(nameof(model.Id), "Id is required.");
            if (string.IsNullOrEmpty(model.Comment))
                result.Errors.Add(nameof(model.Comment), "Comment is required.");
            return result;
        }
    }
}
