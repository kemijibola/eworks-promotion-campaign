using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using System;

namespace EWorksPromotionCampaign.Service.Validators.Admin
{
    public interface IRaffleValidator
    {
        ValidationResult ValidateNewCampaignRaffle(CreateRaffleRewardInputModel model);
    }
    public class RaffleValidator : IRaffleValidator
    {
        public ValidationResult ValidateNewCampaignRaffle(CreateRaffleRewardInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Campaign Raffle is required");
            var result = new ValidationResult();
            if (model.EndDate <= model.StartDate)
                result.Errors.Add(nameof(model.EndDate), "EndDate must be greater than StartDate");
            return result;
        }
    }
}
