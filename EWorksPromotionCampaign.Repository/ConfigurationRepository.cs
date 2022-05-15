using EWorksPromotionCampaign.Shared.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace EWorksPromotionCampaign.Repository
{
    public interface IConfigurationRepository : IBaseRepository<Configuration>
    {
        Task<Configuration> FindByType(string type);
    }
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly string _defaultConnectionString;
        public ConfigurationRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }
        public async Task<long> Create(Configuration newItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var configId = await conn.QueryFirstOrDefaultAsync<long>(@"usp_create_new_configuration", new
            {
                key = newItem.Key,
                value = newItem.Value
            }, commandType: CommandType.StoredProcedure);
            return configId;
        }

        public async Task Delete<TItem>(TItem id)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_delete_configuration", new
                {
                    config_id = id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IReadOnlyCollection<Configuration>> Fetch()
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var configurations = await connection.QueryAsync<Configuration>(
                @"usp_get_configurations",
                commandType: CommandType.StoredProcedure
            );
            return configurations as IReadOnlyCollection<Configuration>;
        }

        public Task<Configuration> FindById<TItem>(TItem id)
        {
            throw new NotImplementedException();
        }

        public async Task<Configuration> FindByType(string type)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var config = await connection.QueryFirstOrDefaultAsync<Configuration>(
                @"usp_get_configuration_by_type", new
                {
                    key = type,
                }, commandType: CommandType.StoredProcedure);
            return config;
        }

        public async Task Update<TItem>(TItem id, Configuration updateItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_configuration", new
                {
                    config_id = id,
                    value = updateItem.Value,
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
