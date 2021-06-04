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
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmailOrPhone(string email, string phone);
        Task<User> GetUserByIdentifier(string identifier);
        Task<User> GetUserByEmail(string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly string _defaultConnectionString;
        public UserRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }
        public async Task<long> Create(User newItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var userId = await conn.QueryFirstOrDefaultAsync<long>(@"usp_create_new_user", new
            {
                first_name = newItem.FirstName,
                middle_name = newItem.MiddleName,
                last_name = newItem.LastName,
                phone = newItem.Phone,
                email = newItem.Email,
                date_of_birth = newItem.DateOfBirth,
                address = newItem.Address,
                password_hash = newItem.PasswordHash,
                password_salt = newItem.PasswordSalt
            },
            commandType: CommandType.StoredProcedure);
            return userId;
        }

        public Task Delete<TItem>(TItem id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<User>> Fetch()
        {
            throw new NotImplementedException();
        }
        public async Task<User> FindById<TItem>(TItem id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                @"usp_get_user_by_id", new
                {
                    id,
                }, commandType: CommandType.StoredProcedure);
            return user;
        }

        public async Task<User> GetUserByEmailOrPhone(string email, string phone)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                @"usp_get_user_by_email_phone", new
                {
                    email,
                    phone
                }, commandType: CommandType.StoredProcedure);
            return user;
        }

        public async Task<User> GetUserByIdentifier(string identifier)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                @"usp_get_user_by_identifier", new
                {
                    identifier,

                }, commandType: CommandType.StoredProcedure);
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var user = await connection.QueryFirstOrDefaultAsync<User>(
                @"usp_get_user_by_email", new
                {
                    email
                }, commandType: CommandType.StoredProcedure);
            return user;
        }

        public async Task Update<TItem>(TItem id, User updateItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_user", new
                {
                    id,
                    first_name = updateItem.FirstName,
                    middle_name = updateItem.MiddleName,
                    last_name = updateItem.LastName,
                    phone = updateItem.Phone,
                    email = updateItem.Email,
                    date_of_birth = updateItem.DateOfBirth,
                    address = updateItem.Address,
                    is_phone_verified = updateItem.IsPhoneVerified,
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
