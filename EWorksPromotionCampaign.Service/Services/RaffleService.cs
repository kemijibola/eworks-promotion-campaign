using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Util;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Service.Validators.Admin;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Models.Admin.Output;
using EWorksPromotionCampaign.Shared.Models.Output.Raffle;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Services
{
    public interface IRaffleService
    {
        Task<Result<FetchRaffleWinnerOutputModel>> GetRaffleWinners(long raffleRewardTypeId);
        Task<Result<FetchRaffleRewardOutputModel>> GetRaffleReward(long raffleRewardTypeId);
        Task<Result<CreateRaffleRewardOutputModel>> CreateRaffleReward(CreateRaffleRewardInputModel model);
        Task<Result<MessageOutputModel>> DeleteRaffleReward(long raffleRewardTypeId);
        Task<Result<FetchRaffleRewardWinnerOutputModel>> GetRaffleRewardWinners(long raffleRewardTypeId);
    }
    public class RaffleService : IRaffleService
    {
        private readonly IRaffleRepository _raffleRepository;
        private static Random _random;
        private readonly IRaffleValidator _raffleValidator;

        public RaffleService(IRaffleRepository raffleRepository, IRaffleValidator raffleValidator)
        {
            _raffleRepository = raffleRepository;
            _random = new Random();
            _raffleValidator = raffleValidator;
        }
        public async Task<Result<FetchRaffleWinnerOutputModel>> GetRaffleWinners(long raffleRewardTypeId)
        {
            var result = await _raffleRepository.FetchEligibleSubscribers(raffleRewardTypeId);
            if (!string.IsNullOrEmpty(result.Result))
            {
                var results = result.Result.Split(':');
                var (statusCode, responseCode) = Helper.MapDbResponseCodeToStatusCode(results[0]);
                if (!responseCode.Equals(ResponseCodes.Success))
                    throw new ServiceException(responseCode, results[1].Trim(), statusCode);
            }
            var eventual = result.Subscribers.Where(s => !HasWonBefore(result.RaffleWinners, s.Phone));
            if (eventual is null && !eventual.Any())
                throw new ServiceException(ResponseCodes.InvalidRequest, "No Subscribers are available for this campaign", 400);

            var winners = SelectWinners(eventual.ToList(), result.NumberofWinners);
            await _raffleRepository.UpdateRaffleWinners(eventual, WinningType.Cash.ToString(), result.Id, result.Amount);
            return new Result<FetchRaffleWinnerOutputModel>(new ValidationResult(), FetchRaffleWinnerOutputModel.FromRaffleWinners(winners));
        }
        public async Task<Result<CreateRaffleRewardOutputModel>> CreateRaffleReward(CreateRaffleRewardInputModel model)
        {
            var validationResult = _raffleValidator.ValidateNewCampaignRaffle(model);
            if (validationResult.IsValid)
            {
                var raffleReward = model.ToCampaignRaffleReward();
                raffleReward.Status = RaffleRewardTypeStatus.NotDrawn.ToString();
                var result = await _raffleRepository.CreateRaffleReward(raffleReward);
                var results = result.Split(':');
                var (statusCode, responseCode) = Helper.MapDbResponseCodeToStatusCode(results[0]);
                if (!responseCode.Equals(ResponseCodes.Success))
                    throw new ServiceException(responseCode, results[1].Trim(), statusCode);
                raffleReward.Id = Convert.ToInt64(results[1].Trim());
                return new Result<CreateRaffleRewardOutputModel>(validationResult, CreateRaffleRewardOutputModel.FromRaffleReward(raffleReward));
            }
            return new Result<CreateRaffleRewardOutputModel>(validationResult, null);
        }
        private static List<RaffleSubscriberViewModel> SelectWinners(List<RaffleSubscriberViewModel> subscribers, int winnersCount)
        {
            var winnersList = new List<int>();
            var winningSubscribers = new List<RaffleSubscriberViewModel>();
            while (winnersList.Count < winnersCount && winnersList.Count < subscribers.Count)
            {
                var winnerIndex = _random.Next(0, subscribers.Count);
                while (winnersList.Any(c => c == winnerIndex))
                {
                    winnerIndex = _random.Next(0, subscribers.Count);
                }
                winnersList.Add(winnerIndex);
            }
            foreach (var item in winnersList)
            {
                winningSubscribers.Add(subscribers[item]);
            }
            return winningSubscribers;
        }
        public static bool HasWonBefore(IReadOnlyCollection<RaffleWinnerViewModel> winners, string phoneNumber)
        {
            return winners.Any(w => w.SubscriberPhone.Equals(phoneNumber));
        }

        public async Task<Result<FetchRaffleRewardOutputModel>> GetRaffleReward(long raffleRewardTypeId)
        {
            var result = await _raffleRepository.FetchRaffleReward(raffleRewardTypeId);
            if (!string.IsNullOrEmpty(result.Result))
            {
                var results = result.Result.Split(':');
                var (statusCode, responseCode) = Helper.MapDbResponseCodeToStatusCode(results[0]);
                if (!responseCode.Equals(ResponseCodes.Success))
                    throw new ServiceException(responseCode, results[1].Trim(), statusCode);
            }
            return new Result<FetchRaffleRewardOutputModel>(new ValidationResult(), FetchRaffleRewardOutputModel.FromRaffleReward(result));
        }

        public async Task<Result<MessageOutputModel>> DeleteRaffleReward(long raffleRewardTypeId)
        {
            var result = await _raffleRepository.DeleteRaffleReward(raffleRewardTypeId);
            if (!string.IsNullOrEmpty(result))
            {
                var results = result.Split(':');
                var (statusCode, responseCode) = Helper.MapDbResponseCodeToStatusCode(results[0]);
                if (!responseCode.Equals(ResponseCodes.Success))
                    throw new ServiceException(responseCode, results[1].Trim(), statusCode);
            }
            return new Result<MessageOutputModel>(new ValidationResult(), MessageOutputModel.FromStringMessage("Raffle reward deleted successfully"));
        }

        public async Task<Result<FetchRaffleRewardWinnerOutputModel>> GetRaffleRewardWinners(long raffleRewardTypeId)
        {
            var result = await _raffleRepository.FetchRaffleRewardWinners(raffleRewardTypeId);
            if (!string.IsNullOrEmpty(result.Result))
            {
                var results = result.Result.Split(':');
                var (statusCode, responseCode) = Helper.MapDbResponseCodeToStatusCode(results[0]);
                if (!responseCode.Equals(ResponseCodes.Success))
                    throw new ServiceException(responseCode, results[1].Trim(), statusCode);
            }
            return new Result<FetchRaffleRewardWinnerOutputModel>(new ValidationResult(), FetchRaffleRewardWinnerOutputModel.FromRaffleWinners(result.Subscribers));
        }
    }
}
