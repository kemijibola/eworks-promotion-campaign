using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Validators.Admin
{
    public interface ICampaignValidator
    {
        ValidationResult ValidateNewCampaign(CreateCampaignInputModel model);
        ValidationResult ValidateExistingCampaign(EditCampaignInputModel model);
    }
    public class CampaignValidator : ICampaignValidator
    {
        public ValidationResult ValidateExistingCampaign(EditCampaignInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Campaign is required");
            var result = new ValidationResult();
            if (model.Id < 1)
                result.Errors.Add(nameof(model.EndDate), "Provide a valid Id");
            if (model.EndDate <= model.StartDate)
                result.Errors.Add(nameof(model.EndDate), "EndDate must be greater than Start Date");
            if (model.EndDate < DateTime.UtcNow)
                result.Errors.Add(nameof(model.EndDate), "EndDate cannot be in the past");
            if (model.MinEntryAmount < 1)
                result.Errors.Add(nameof(model.MinEntryAmount), "MinEntryAmount is required");
            if (model.MaxEntryAmount < 1)
                result.Errors.Add(nameof(model.MaxEntryAmount), "MaxEntryAmount is required");
            if (model.MinEntryAmount > model.MaxEntryAmount)
                result.Errors.Add(nameof(model.MaxEntryAmount), "MaxEntryAmount must be greater than MinEntryAmount");
            return result;
        }

        public ValidationResult ValidateNewCampaign(CreateCampaignInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Campaign is required");
            var result = new ValidationResult();
            if (model.EndDate <= model.StartDate)
                result.Errors.Add(nameof(model.EndDate), "EndDate must be greater than Start Date");
            if (model.EndDate < DateTime.UtcNow)
                result.Errors.Add(nameof(model.EndDate), "EndDate cannot be in the past");
            if (model.MinEntryAmount < 1)
                result.Errors.Add(nameof(model.MinEntryAmount), "MinEntryAmount is required");
            if (model.MaxEntryAmount < 1)
                result.Errors.Add(nameof(model.MaxEntryAmount), "MaxEntryAmount is required");
            if (model.MinEntryAmount > model.MaxEntryAmount)
                result.Errors.Add(nameof(model.MaxEntryAmount), "MaxEntryAmount must be greater than MinEntryAmount");
            return result;
        }
    }
}
