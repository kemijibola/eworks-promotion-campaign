using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Validators.Admin
{
    public interface IPermissionValidator
    {
        ValidationResult ValidateUpdatePermissionStatus(UpdatePermissionStatusInputModel model);
        ValidationResult ValidateNewPermission(CreatePermissionInputModel model);
    }
    public class PermissionValidator : IPermissionValidator
    {
        public ValidationResult ValidateNewPermission(CreatePermissionInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Permission is required");
            var result = new ValidationResult();
            if (string.IsNullOrEmpty(model.PermissionName))
                result.Errors.Add(nameof(model.PermissionName), "PermissionName is required.");
            if (string.IsNullOrEmpty(model.PermissionDescription))
                result.Errors.Add(nameof(model.PermissionDescription), "PermissionDescription is required.");
            return result;
        }

        public ValidationResult ValidateUpdatePermissionStatus(UpdatePermissionStatusInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Permission is required");
            var result = new ValidationResult();
            if (model.PermissionId < 1)
                result.Errors.Add(nameof(model.PermissionId), "PermissionId is required.");
            if (string.IsNullOrEmpty(model.Comment))
                result.Errors.Add(nameof(model.Comment), "Comment is required.");
            return result;
        }
    }
}
