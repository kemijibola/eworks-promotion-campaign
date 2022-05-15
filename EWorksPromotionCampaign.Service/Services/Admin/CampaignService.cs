using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Util;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Service.Validators.Admin;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Models.Admin.Output;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Services.Admin
{
    public interface ICampaignService
    {
        Task<AddResult<CreateCampaignOutputModel>> CreateCampaign(CreateCampaignInputModel model);
        Task<Result<FetchCampaignOutputModel>> GetCampaignById(long id, bool withCampaignRewards);
        Task<Result<EditCampaignOutputModel>> EditCampaign(EditCampaignInputModel model);
        Task<Result<MessageOutputModel>> StartCampaign(long id);
        Task<Result<MessageOutputModel>> PauseCampaign(long id);
    }
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignValidator _campaignValidator;
        private readonly ICampaignRepository _campaignRepository;
        private readonly ICampaignRewardRepository _campaignRewardRepository;
        public CampaignService(ICampaignValidator campaignValidator, ICampaignRepository campaignRepository, ICampaignRewardRepository campaignRewardRepository)
        {
            _campaignRepository = campaignRepository;
            _campaignValidator = campaignValidator;
            _campaignRewardRepository = campaignRewardRepository;
        }
        public async Task<AddResult<CreateCampaignOutputModel>> CreateCampaign(CreateCampaignInputModel model)
        {
            var validationResult = _campaignValidator.ValidateNewCampaign(model);
            var existingCampaign = await _campaignRepository.FindByName(model.Name);
            if (validationResult.IsValid && existingCampaign is null)
            {
                var campaign = model.ToCampaign();
                campaign.Id = await _campaignRepository.Create(campaign);
                return new AddResult<CreateCampaignOutputModel>(validationResult, false, CreateCampaignOutputModel.FromCampaign(campaign));
            }
            return new AddResult<CreateCampaignOutputModel>(validationResult, validationResult.IsValid, null);
        }
        public async Task<Result<EditCampaignOutputModel>> EditCampaign(EditCampaignInputModel model)
        {
            var validationResult = _campaignValidator.ValidateExistingCampaign(model);
            var existingCampaign = await GetCampaignById(model.Id, false);
            if (existingCampaign is null)
                throw new ServiceException(ResponseCodes.NotFound, "Campaign not found", 404);
            if (validationResult.IsValid)
            {
                if (!model.Name.Equals(existingCampaign.Data.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    var newCampaign = await _campaignRepository.FindByName(model.Name);
                    if (newCampaign is not null)
                        throw new ServiceException(ResponseCodes.Conflict, "Campaign name exists", 409);
                }
                var campaign = model.ToCampaign();
                await _campaignRepository.Update(campaign.Id, campaign);
                campaign.Status = existingCampaign.Data.Status;
                campaign.CreatedAt = existingCampaign.Data.CreatedAt;
                campaign.UpdatedAt = existingCampaign.Data.UpdatedAt;
                return new Result<EditCampaignOutputModel>(validationResult, EditCampaignOutputModel.FromCampaign(campaign));
            }
            return new Result<EditCampaignOutputModel>(validationResult, null);
        }

        public async Task<Result<FetchCampaignOutputModel>> GetCampaignById(long id, bool withCampaignRewards)
        {
            var campaign = await _campaignRepository.FindById(id);
            if (campaign is null)
                throw new ServiceException(ResponseCodes.NotFound, "Campaign not found", 404);
            if (withCampaignRewards)
            {
                var campaignRewards = await _campaignRewardRepository.FetchByCampaignId(campaign.Id);
                campaign.WithCampignRewards(campaignRewards);
            }
            return new Result<FetchCampaignOutputModel>(new ValidationResult(), FetchCampaignOutputModel.FromCampaign(campaign));
        }

        public async Task<Result<MessageOutputModel>> PauseCampaign(long id)
        {
            var campaignResult = await GetCampaignById(id, true);
            var campaign = HelperModel.ToCampaign(campaignResult.Data);
            if (campaign.Status.Equals(CampaignStatus.expired.ToString(), StringComparison.InvariantCultureIgnoreCase))
                throw new ServiceException(ResponseCodes.InvalidRequest, "Campaign has expired", 400);

            campaign.Status = CampaignStatus.inactive.ToString();
            foreach (var item in campaignResult.Data.CampaignRewards)
            {
                if (item.StartMode.Equals(StartMode.Automatic.ToString()))
                    item.Status = RewardStatus.Inactive.ToString();
            }
            await _campaignRepository.Update(campaign, campaignResult.Data.CampaignRewards);
            return new Result<MessageOutputModel>(new ValidationResult(), MessageOutputModel.FromStringMessage("Campaign paused successfully"));
        }

        public async Task<Result<MessageOutputModel>> StartCampaign(long id)
        {
            var campaignResult = await GetCampaignById(id, true);
            var campaign = HelperModel.ToCampaign(campaignResult.Data);
            if (campaign.Status.Equals(CampaignStatus.expired.ToString(), StringComparison.InvariantCultureIgnoreCase))
                throw new ServiceException(ResponseCodes.InvalidRequest, "Campaign has expired", 400);

            campaign.Status = CampaignStatus.ongoing.ToString();
            foreach (var item in campaignResult.Data.CampaignRewards)
            {
                if (item.StartMode.Equals(StartMode.Automatic.ToString()))
                    item.Status = RewardStatus.Active.ToString();
            }
            await _campaignRepository.Update(campaign, campaignResult.Data.CampaignRewards);
            return new Result<MessageOutputModel>(new ValidationResult(), MessageOutputModel.FromStringMessage("Campaign started successfully"));
        }
    }
}
