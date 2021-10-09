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
    public interface IPaymentRepository
    {
        Task<string> Create(Payment payment, string userEmail); 
    }
    public class PaymentRepository : IPaymentRepository
    {
        private readonly string _defaultConnectionString;
        public PaymentRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }

        public async Task<string> Create(Payment payment, string userEmail)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var result = await conn.QueryFirstOrDefaultAsync<string>(@"usp_create_new_payment", new
            {
                order_id = payment.OrderId,
                user_email = userEmail,
                amount = payment.Amount,
                description = payment.Description,
                gateway_response = payment.GatewayReponse,
                gateway_reference = payment.GatewayReference,
                status = payment.Status,
                payment_status = payment.PaymentStatus,
                transaction_date = payment.TransactionDate,
                additional_info = payment.AdditionalInfo
            }, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
