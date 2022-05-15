using Dapper;
using EWorksPromotionCampaign.Shared.Models.Domain;
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
    public interface IOrderRepository
    {
        Task<string> Create(Order newItem);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly string _defaultConnectionString;
        public OrderRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }
        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Order>> Fetch()
        {
            throw new NotImplementedException();
        }

        public Task<Order> FindById(long id)
        {
            throw new NotImplementedException();
        }
        public async Task<string> Create(Order newItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var result = await conn.QueryFirstOrDefaultAsync<string>(@"usp_create_new_order", new
            {
                user_id = newItem.UserId,
                amount = newItem.Amount,
                campaign_id = newItem.CampaignId
            }, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
