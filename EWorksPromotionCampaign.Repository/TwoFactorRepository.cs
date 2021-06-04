using Dapper;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Repository
{
    public interface ITwoFactorRepository : IBaseRepository<ITwoFactorAuthentication>
    {
        Task<ITwoFactorAuthentication> GetTokenReqestByToken(string token);
    }
    public class TwoFactorRepository : ITwoFactorRepository
    {
        private readonly string _defaultConnectionString;
        public TwoFactorRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }

        public async Task<long> Create(ITwoFactorAuthentication newItem)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return await connection.ExecuteAsync(
                @"usp_insert_token_request", new
                {
                    requester = newItem.Requester,
                    token = newItem.Token,
                    salt = newItem.Salt,
                    type_of_token = newItem.TypeOfToken,
                    request_id = newItem.RequestId
                }, commandType: CommandType.StoredProcedure);
        }

        public async Task Delete<TItem>(TItem id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await connection.ExecuteAsync(
                @"usp_delete_token_request_by_request_id", new
                {
                    request_id = id
                }, commandType: CommandType.StoredProcedure);
        }

        public Task<IReadOnlyCollection<ITwoFactorAuthentication>> Fetch()
        {
            throw new NotImplementedException();
        }

        public async Task<ITwoFactorAuthentication> FindById<TItem>(TItem id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var tokenData = await connection.QueryFirstOrDefaultAsync<TwoFactorAuthenticationDto>(
                @"usp_get_token_request_by_request_id", new
                {
                    request_id = id
                }, commandType: CommandType.StoredProcedure);
            return tokenData;
        }

        public async Task<ITwoFactorAuthentication> GetTokenReqestByToken(string token)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var tokenData = await connection.QueryFirstOrDefaultAsync<TwoFactorAuthenticationDto>(
                @"usp_get_token_request_by_token", new
                {
                    token
                }, commandType: CommandType.StoredProcedure);
            return tokenData;
        }
        public Task Update<TItem>(TItem id, ITwoFactorAuthentication updateItem)
        {
            throw new NotImplementedException();
        }
    }
}
