using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Validators.Admin
{
    public interface ICampaignRewardValidator
    {
        ValidationResult ValidateNewCampaignReward(CreateCampaignRewardInputModel model, Campaign campaign);
    }
    public class CampaignRewardValidator : ICampaignRewardValidator
    {
        public ValidationResult ValidateNewCampaignReward(CreateCampaignRewardInputModel model, Campaign campaign)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Campaign Reward is required");
            var result = new ValidationResult();
            if (campaign is null)
                result.Errors.Add(nameof(model.CampaignId), "Campaign not found");
            if (campaign.Status.Equals(CampaignStatus.expired.ToString(), StringComparison.InvariantCultureIgnoreCase))
                result.Errors.Add(nameof(model.CampaignId), "Campaign has expired");
            if (model.EndDate <= model.StartDate)
                result.Errors.Add(nameof(model.EndDate), "EndDate must be greater than Start Date");
            if (model.EndDate < DateTime.UtcNow)
                result.Errors.Add(nameof(model.EndDate), "EndDate cannot be in the past.");
            if (model.StartDate < campaign.StartDate)
                result.Errors.Add(nameof(model.EndDate), $"This campaign does not start till {campaign.StartDate:dd/MM/yyyy hh:mm:ss}, winning must start on or after then.");
            if (model.EndDate > campaign.EndDate)
                result.Errors.Add(nameof(model.EndDate), $"Campaign ends {campaign.EndDate:dd/MM/yyyy hh:mm:ss}, winning must end on or before then.");
            if (model.ScheduleWinningRule.Equals(ScheduleWinningRule.EveryNthSubscriber.ToString()) && model.NthSubscriberValue < 2)
                result.Errors.Add(nameof(model.EndDate), "Invalid value of Nth. N should be 2 or greater.");
            if (!Enum.TryParse(model.ScheduleWinningRule, true, out ScheduleWinningRule _))
                result.Errors.Add(nameof(model.ScheduleWinningRule), "Schedule Winning Rule is invalid.");
            if (!Enum.TryParse(model.WinningType, true, out WinningType _))
                result.Errors.Add(nameof(model.WinningType), "Winning Type is invalid.");
            if (!Enum.TryParse(model.CampaignType, true, out CampaignType _))
                result.Errors.Add(nameof(model.CampaignType), "Campaign Type is invalid.");
            if (!Enum.TryParse(model.ScheduleType, true, out ScheduleType _))
                result.Errors.Add(nameof(model.ScheduleType), "Schedule Type is invalid.");
            if (!Enum.TryParse(model.StartMode, true, out StartMode _))
                result.Errors.Add(nameof(model.StartMode), "StartMode is invalid.");
            return result;
        }
    }
}
