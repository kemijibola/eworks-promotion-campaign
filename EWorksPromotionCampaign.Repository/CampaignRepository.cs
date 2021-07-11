using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EWorksPromotionCampaign.Repository
{
    public interface ICampaignRepository : IBaseRepository<Campaign>
    {
        Task<Campaign> FindByName(string name);
        Task Update(Campaign updateItem, IReadOnlyCollection<CampaignReward> campaignRewards);
    }
    public class CampaignRepository : ICampaignRepository
    {
        private readonly string _defaultConnectionString;
        public CampaignRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }
        public async Task<long> Create(Campaign newItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var campaignId = await conn.QueryFirstOrDefaultAsync<long>(@"usp_create_new_campaign", new
            {
                name = newItem.Name,
                start_date = newItem.StartDate,
                end_date = newItem.EndDate,
                min_entry_amount = newItem.MinEntryAmount,
                max_entry_amount = newItem.MaxEntryAmount,
                status = newItem.Status
            }, commandType: CommandType.StoredProcedure);
            return campaignId;
        }

        public Task Delete<TItem>(TItem id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<Campaign>> Fetch()
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var campaigns = await connection.QueryAsync<Campaign>(
                @"usp_fetch_campaigns",
                commandType: CommandType.StoredProcedure
            );
            return campaigns as IReadOnlyCollection<Campaign>;
        }

        public async Task<Campaign> FindById<TItem>(TItem id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return await connection.QueryFirstOrDefaultAsync<Campaign>(
                @"usp_fetch_campaign_by_id",
                new
                {
                    id
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Campaign> FindByName(string name)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var config = await connection.QueryFirstOrDefaultAsync<Campaign>(
                @"usp_get_campaign_by_name", new
                {
                    name,
                }, commandType: CommandType.StoredProcedure);
            return config;
        }

        public async Task Update<TItem>(TItem id, Campaign updateItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_campaign", new
                {
                    id,
                    name = updateItem.Name,
                    start_date = updateItem.StartDate,
                    end_date = updateItem.EndDate,
                    min_entry_amount = updateItem.MinEntryAmount,
                    max_entry_amount = updateItem.MaxEntryAmount
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task Update(Campaign updateItem, IReadOnlyCollection<CampaignReward> campaignRewards)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();
            try
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                await conn.ExecuteAsync(
                    @"usp_update_campaign_status",
                    new
                    {
                        id = updateItem.Id,
                        status = updateItem.Status
                    },
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction
                 );
                foreach (var item in campaignRewards)
                    await UpdateRewardStatus(item, conn, transaction);
                await transaction.CommitAsync();
            }
            catch(Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static async Task UpdateRewardStatus(
            CampaignReward campaignReward,
            IDbConnection connection,
            IDbTransaction transaction
        )
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await connection.ExecuteAsync(
                @"usp_update_campaign_reward_status",
                new
                {
                    campaignReward.Id,
                    status = campaignReward.Status
                },
                commandType: CommandType.StoredProcedure,
                transaction: transaction
            );
        }
    }
}
