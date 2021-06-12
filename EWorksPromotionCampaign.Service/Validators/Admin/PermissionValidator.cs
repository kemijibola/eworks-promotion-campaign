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
    }
    public class PermissionValidator : IPermissionValidator
    {
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
