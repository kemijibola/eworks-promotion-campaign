using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Validators.Admin
{
    public interface IRoleValidator
    {
        ValidationResult ValidateNewRole(CreateRoleInputModel model);
        ValidationResult ValidateUpdateRoleStatus(UpdateRoleStatusInputModel model);
        ValidationResult ValidateResetRolePermission(int roleId, int[] permissions);
        ValidationResult ValidateUpdateRole(UpdateRoleInputModel model);
    }
    public class RoleValidator : IRoleValidator
    {
        public ValidationResult ValidateNewRole(CreateRoleInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Role is required");
            var result = new ValidationResult();
            if (string.IsNullOrEmpty(model.RoleName))
                result.Errors.Add(nameof(model.RoleName), "RoleName is required.");
            if (string.IsNullOrEmpty(model.RoleDescription))
                result.Errors.Add(nameof(model.RoleDescription), "RoleDescription is required.");
            return result;
        }

        public ValidationResult ValidateResetRolePermission(int roleId, int[] permissions)
        {
            var result = new ValidationResult();
            if (roleId < 1)
                result.Errors.Add(nameof(roleId), "RoleId is required.");
            if (permissions.Length < 1)
                result.Errors.Add(nameof(permissions), "Provide at least 1 permission.");
            return result;
        }

        public ValidationResult ValidateUpdateRole(UpdateRoleInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Role is required");
            var result = new ValidationResult();
            if (model.Id < 1)
                result.Errors.Add(nameof(model.RoleName), "Id is required.");
            if (string.IsNullOrEmpty(model.RoleName))
                result.Errors.Add(nameof(model.RoleName), "RoleName is required.");
            if (string.IsNullOrEmpty(model.RoleDescription))
                result.Errors.Add(nameof(model.RoleDescription), "RoleDescription is required.");
            return result;
        }

        public ValidationResult ValidateUpdateRoleStatus(UpdateRoleStatusInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Role is required");
            var result = new ValidationResult();
            if (model.RoleId < 1)
                result.Errors.Add(nameof(model.RoleId), "RoleId is required.");
            if (string.IsNullOrEmpty(model.Comment))
                result.Errors.Add(nameof(model.Comment), "Comment is required.");
            return result;
        }
    }
}
