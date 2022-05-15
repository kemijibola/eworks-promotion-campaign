using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Service.Validators.Admin;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models;
using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Models.Admin.Output;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Services.Admin
{
    public interface ICampaignRewardService
    {
        Task<Result<CreateCampaignRewardOutputModel>> AddCampaignReward(CreateCampaignRewardInputModel model);
        Task<Result<MessageOutputModel>> StartCampaignReward(long id);
        Task<Result<MessageOutputModel>> PauseCampaignReward(long id);
        Task<Result<MessageOutputModel>> DeleteCampaignReward(long id);
    }
    public class CampaignRewardService : ICampaignRewardService
    {
        private readonly ICampaignRewardValidator _campaignRewardValidator;
        private readonly ICampaignRewardRepository _campaignRewardRepository;
        private readonly ICampaignService _campaignService;
        public CampaignRewardService(ICampaignRewardValidator campaignRewardValidator, ICampaignRewardRepository campaignRewardRepository, ICampaignService campaignService)
        {
            _campaignRewardValidator = campaignRewardValidator;
            _campaignRewardRepository = campaignRewardRepository;
            _campaignService = campaignService;
        }
        public async Task<Result<CreateCampaignRewardOutputModel>> AddCampaignReward(CreateCampaignRewardInputModel model)
        {
            var existingCampaignResult = await _campaignService.GetCampaignById(model.CampaignId, true);
            var campaign = HelperModel.ToCampaign(existingCampaignResult.Data);
            var validationResult = _campaignRewardValidator.ValidateNewCampaignReward(model, campaign);
            if (WinningTypeDateOverlaps(campaign.CampaignRewards, model.StartDate, campaign.Id, model.WinningType))
                throw new ServiceException(ResponseCodes.Conflict, "Winning Type already exists between this time frame", 409);
            if (CashRewardExists(campaign.CampaignRewards, model.Amount))
                throw new ServiceException(ResponseCodes.Conflict, "There's already a winning with the amount specified", 409);
            if (validationResult.IsValid)
            {
                var campaignRewardStatus = campaign.Status.Equals(CampaignStatus.ongoing.ToString(), StringComparison.InvariantCultureIgnoreCase) ?
                    RewardStatus.Active.ToString() : RewardStatus.Inactive.ToString();
                var campaignReward = model.ToCampaignReward(campaignRewardStatus);
                campaignReward.Id = await _campaignRewardRepository.Create(campaignReward);
                return new Result<CreateCampaignRewardOutputModel>(validationResult, CreateCampaignRewardOutputModel.FromCampaignReward(campaignReward));
            }
            return new Result<CreateCampaignRewardOutputModel>(validationResult, null);
        }

        private static bool WinningTypeDateOverlaps(IReadOnlyCollection<CampaignReward> campaignRewards, DateTime startDate, long campaignId, string winningType)
        {
            return campaignRewards
                    .Any(reward => reward.WinningType.Equals(winningType) &&
                    reward.CampaignId.Equals(campaignId) &&
                    reward.StartDate <= startDate && reward.EndDate >= startDate);
        }
        private static bool CashRewardExists(IReadOnlyCollection<CampaignReward> campaignRewards, decimal amount)
        {
            return campaignRewards
                    .Any(reward => reward.WinningType.Equals(WinningType.Cash.ToString()) && reward.Amount.Equals(amount));
        }

        public async Task<Result<MessageOutputModel>> PauseCampaignReward(long id)
        {
            var existingCampaignReward = await _campaignRewardRepository.FindById(id);
            if (existingCampaignReward is null)
                throw new ServiceException(ResponseCodes.NotFound, "Campaign Reward not found", 404);
            if (DateTime.UtcNow >= existingCampaignReward.EndDate)
                throw new ServiceException(ResponseCodes.InvalidRequest, "Campaign Reward is in the past", 400);
            var existingCampaignResult = await _campaignService.GetCampaignById(existingCampaignReward.CampaignId, false);
            var campaign = HelperModel.ToCampaign(existingCampaignResult.Data);
            if (campaign.Status.Equals(CampaignStatus.expired.ToString(), StringComparison.InvariantCultureIgnoreCase))
                throw new ServiceException(ResponseCodes.InvalidRequest, "Campaign has expired", 400);

            if (existingCampaignReward.StartMode.Equals(StartMode.Automatic.ToString(), StringComparison.InvariantCultureIgnoreCase))
                existingCampaignReward.Status = RewardStatus.Inactive.ToString();

            await _campaignRewardRepository.UpdateCampaignRewardStatus(existingCampaignReward);
            return new Result<MessageOutputModel>(new ValidationResult(), MessageOutputModel.FromStringMessage("Campaign Reward paused successfully"));
        }

        public async Task<Result<MessageOutputModel>> StartCampaignReward(long id)
        {
            var existingCampaignReward = await _campaignRewardRepository.FindById(id);
            if (existingCampaignReward is null)
                throw new ServiceException(ResponseCodes.NotFound, "Campaign Reward not found", 404);
            if (DateTime.UtcNow >= existingCampaignReward.EndDate)
                throw new ServiceException(ResponseCodes.InvalidRequest, "Campaign Reward is in the past", 400);

            var existingCampaignResult = await _campaignService.GetCampaignById(existingCampaignReward.CampaignId, false);
            var campaign = HelperModel.ToCampaign(existingCampaignResult.Data);
            if (campaign.Status.Equals(CampaignStatus.expired.ToString(), StringComparison.InvariantCultureIgnoreCase))
                throw new ServiceException(ResponseCodes.InvalidRequest, "Campaign has expired", 400);

            if (existingCampaignReward.StartMode.Equals(StartMode.Automatic.ToString(), StringComparison.InvariantCultureIgnoreCase))
                existingCampaignReward.Status = RewardStatus.Active.ToString();

            await _campaignRewardRepository.UpdateCampaignRewardStatus(existingCampaignReward);
            return new Result<MessageOutputModel>(new ValidationResult(), MessageOutputModel.FromStringMessage("Campaign Reward started successfully"));
        }

        public async Task<Result<MessageOutputModel>> DeleteCampaignReward(long id)
        {
            await _campaignRewardRepository.Delete(id);
            return new Result<MessageOutputModel>(new ValidationResult(), MessageOutputModel.FromStringMessage("Campaign Reward deleted successfully"));
        }
    }
}
