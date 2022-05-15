using Dapper;
using EWorksPromotionCampaign.Shared.Models;
using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Repository
{
    public interface IRaffleRepository
    {
        Task<string> CreateRaffleReward(CampaignRaffleReward newItem);
        Task<FetchRaffleReward> FetchRaffleReward(long id);
        Task<RaffleRewardViewModel> FetchEligibleSubscribers(long raffleRewardId);
        Task UpdateRaffleWinners(IEnumerable<RaffleSubscriberViewModel> subscribers, string winningType, long raffleRewardTypeId, decimal raffleRewardAmount);
        Task<string> DeleteRaffleReward(long raffleReward);
        Task<RaffleWinnerDetailViewModel> FetchRaffleRewardWinners(long raffleRewardTypeId);
    }
    public class RaffleRepository : IRaffleRepository
    {
        private readonly string _defaultConnectionString;
        public RaffleRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }
        public async Task<RaffleRewardViewModel> FetchEligibleSubscribers(long raffleRewardId)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var query = await connection.QueryMultipleAsync(
                @"usp_get_eligible_raffle_subscribers", 
                new 
                {
                    raffle_reward_id = raffleRewardId
                },
                commandType: CommandType.StoredProcedure
            );
            var result = query.Read<RaffleRewardViewModel>();
            var raffleReward = result.FirstOrDefault();
            if (string.IsNullOrEmpty(raffleReward.Result))
            {
                raffleReward.RaffleWinners = query.Read<RaffleWinnerViewModel>().ToList();
                raffleReward.Subscribers = query.Read<RaffleSubscriberViewModel>().ToList();
            }
            return raffleReward;
        }

        public async Task UpdateRaffleWinners(IEnumerable<RaffleSubscriberViewModel> subscribers, string winningType, long raffleRewardTypeId, decimal raffleRewardAmount)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var subscriberDT = subscribers.ToDataTable();
            var parameters = new DynamicParameters();
            parameters.Add("@raffle_reward_type_id", raffleRewardTypeId);
            parameters.Add("@raffle_reward_amount", raffleRewardAmount);
            parameters.Add("@winning_type", winningType);
            parameters.Add("@subscribers", subscriberDT.AsTableValuedParameter("subscriber_type"));
            await conn.ExecuteAsync(
                @"usp_update_raffle_winners",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<string> CreateRaffleReward(CampaignRaffleReward newItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var result = await conn.QueryFirstOrDefaultAsync<string>(@"usp_create_new_campaign_raffle_reward", new
            {
                campaign_id = newItem.CampaignId,
                start_date = newItem.StartDate,
                end_date = newItem.EndDate,
                number_of_winners = newItem.NumberofWinners,
                amount = newItem.Amount,
                status = newItem.Status
            }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<FetchRaffleReward> FetchRaffleReward(long id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return await connection.QueryFirstOrDefaultAsync<FetchRaffleReward>(
                @"usp_fetch_raffle_reward_by_id",
                new
                {
                    id
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<string> DeleteRaffleReward(long raffleReward)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var result = await conn.QueryFirstOrDefaultAsync<string>(@"usp_delete_raffle_reward", new
            {
                id = raffleReward
            }, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<RaffleWinnerDetailViewModel> FetchRaffleRewardWinners(long raffleRewardTypeId)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var query = await connection.QueryMultipleAsync(
                @"usp_get_raffle_reward_winners",
                new
                {
                    raffle_reward_id = raffleRewardTypeId
                },
                commandType: CommandType.StoredProcedure
            );
            var result = query.Read<RaffleWinnerDetailViewModel>();
            var winner = result.FirstOrDefault();
            if (string.IsNullOrEmpty(winner.Result))
            {
                winner.Subscribers = query.Read<RaffleSubscriberViewModel>().ToList();
            }
            return winner;
        }
    }
}
