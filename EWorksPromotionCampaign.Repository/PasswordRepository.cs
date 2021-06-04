using Dapper;
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
    public interface IPasswordRepository
    {
        Task UpdateUserPasswordData(string email, string passwordHash, string passwordSalt);
    }
    public class PasswordRepository : IPasswordRepository
    {
        private readonly string _defaultConnectionString;
        public PasswordRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }

        public async Task UpdateUserPasswordData(string email, string passwordHash, string passwordSalt)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_user_password", new
                {
                    email,
                    password_hash = passwordHash,
                    password_salt = passwordSalt
                }, commandType: CommandType.StoredProcedure);
        }
    }
}
