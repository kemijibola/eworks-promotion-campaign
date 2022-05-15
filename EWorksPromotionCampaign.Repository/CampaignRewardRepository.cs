using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace EWorksPromotionCampaign.Repository
{
    public interface ICampaignRewardRepository : IBaseRepository<CampaignReward>
    {
        Task<IReadOnlyCollection<CampaignReward>> FetchByCampaignId(long id);
        Task UpdateCampaignRewardStatus(CampaignReward campaignReward);
    }
    public class CampaignRewardRepository : ICampaignRewardRepository
    {
        private readonly string _defaultConnectionString;
        public CampaignRewardRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }
        public async Task<long> Create(CampaignReward newItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var campaignRewardId = await conn.QueryFirstOrDefaultAsync<long>(@"usp_create_new_campaign_reward", new
            {
                campaign_id = newItem.CampaignId,
                winning_type = newItem.WinningType,
                campaign_type = newItem.CampaignType,
                start_mode = newItem.StartMode,
                schedule_type = newItem.ScheduleType,
                amount = newItem.Amount,
                start_date = newItem.StartDate,
                end_date = newItem.EndDate,
                status = newItem.Status,
                number_of_winners = newItem.NumberOfWinners,
                schedule_winning_rule = newItem.ScheduleWinningRule,
                nth_subscriber_value = newItem.NthSubscriberValue,
            }, commandType: CommandType.StoredProcedure);
            return campaignRewardId;
        }

        public async Task Delete<TItem>(TItem id)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_delete_campaign_reward",
                new
                {
                    id,
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IReadOnlyCollection<CampaignReward>> Fetch()
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var campaignRewards = await connection.QueryAsync<CampaignReward>(
                @"usp_get_campaign_rewards",
                commandType: CommandType.StoredProcedure
            );
            return campaignRewards as IReadOnlyCollection<CampaignReward>;
        }

        public async Task<IReadOnlyCollection<CampaignReward>> FetchByCampaignId(long id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var configurations = await connection.QueryAsync<CampaignReward>(
                @"usp_get_campaign_rewards_by_campaign_id", new {campaign_id = id},
                commandType: CommandType.StoredProcedure
            );
            return configurations as IReadOnlyCollection<CampaignReward>;
        }

        public async Task<CampaignReward> FindById<TItem>(TItem id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return await connection.QueryFirstOrDefaultAsync<CampaignReward>(
                @"usp_fetch_campaign_reward_by_id",
                new
                {
                    id
                },
                commandType: CommandType.StoredProcedure);
        }

        public Task Update<TItem>(TItem id, CampaignReward updateItem)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCampaignRewardStatus(CampaignReward campaignReward)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_campaign_reward_status",
                new
                {
                    campaignReward.Id,
                    status = campaignReward.Status
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
