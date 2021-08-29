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
using AdminUser = EWorksPromotionCampaign.Shared.Models.Admin.Domain.User;
using EWorksPromotionCampaign.Shared.Models.Admin;
using System.Dynamic;

namespace EWorksPromotionCampaign.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByEmailOrPhone(string email, string phone);
        Task<User> GetUserByIdentifier(string identifier);
        Task<User> GetUserByEmail(string email);
        Task<AdminUser> GetAdminUserByIdetifier(string identifier);
        Task<int> CreateAdminUser(AdminUser user);
        Task<AdminUser> GetAdminUserByEmailOrPhone(string email, string phone);
        Task<AdminUser> GetAdminUserByEmail(string email);
        Task UpdateAdminUser(AdminUser user);
        Task<AdminUser> FindAdminById(long id);
        Task UpdateAdminUserStatus(AdminUser user);
        Task UpdateAdminUserDisabledStatus(AdminUser user);
        Task<IReadOnlyCollection<AdminUserOverview>> GetAdminUserOverviews(int pageNumber, int pageSize, string searchText);
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
                    is_phone_verified = updateItem.IsPhoneVerified
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<AdminUser> GetAdminUserByIdetifier(string identifier)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var user = await connection.QueryFirstOrDefaultAsync<AdminUser>(
                @"usp_get_admin_user_by_identifier", new
                {
                    identifier,

                }, commandType: CommandType.StoredProcedure);
            return user;
        }

        public async Task<int> CreateAdminUser(AdminUser user)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var userId = await connection.QueryFirstOrDefaultAsync<int>(@"usp_create_new_admin_user", new
            {
                first_name = user.FirstName,
                last_name = user.LastName,
                phone = user.Phone,
                email = user.Email,
                role_id = user.RoleId,
                password_hash = user.PasswordHash,
                password_salt = user.PasswordSalt
            },
                commandType: CommandType.StoredProcedure
            );
            return userId;
        }

        public async Task<AdminUser> GetAdminUserByEmailOrPhone(string email, string phone)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var user = await connection.QueryFirstOrDefaultAsync<AdminUser>(
                @"usp_get_admin_user_by_email_phone", new
                {
                    email,
                    phone
                }, commandType: CommandType.StoredProcedure);
            return user;
        }

        public async Task<AdminUser> GetAdminUserByEmail(string email)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var user = await connection.QueryFirstOrDefaultAsync<AdminUser>(
                @"usp_get_admin_user_by_email", new
                {
                    email
                }, commandType: CommandType.StoredProcedure);
            return user;
        }

        public async Task UpdateAdminUser(AdminUser user)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_admin_user", new
                {
                    user_id = user.Id,
                    first_name = user.FirstName,
                    last_name = user.LastName,
                    phone = user.Phone,
                    email = user.Email,
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<AdminUser> FindAdminById(long id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var user = await connection.QueryFirstOrDefaultAsync<AdminUser>(
                @"usp_get_admin_user_by_id", new
                {
                    id,
                }, commandType: CommandType.StoredProcedure);
            return user;
        }

        public async Task UpdateAdminUserStatus(AdminUser user)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_admin_user_status", new
                {
                    user_id = user.Id,
                    status = user.Status,
                    comment = user.StatusComment,
                    updated_by = user.StatusUpdatedBy
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAdminUserDisabledStatus(AdminUser user)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_user_disabled_status", new
                {
                    user_id = user.Id,
                    status = user.IsDisabled,
                    comment = user.DisabledComment,
                    disabled_by = user.DisabledBy
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IReadOnlyCollection<AdminUserOverview>> GetAdminUserOverviews(int pageNumber, int pageSize, string searchText)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            dynamic parameters = new ExpandoObject();
            parameters.page_number = pageNumber;
            parameters.page_size = pageSize;
            if (!string.IsNullOrEmpty(searchText))
              parameters.search_text = searchText;

            var adminUsers = await connection.QueryAsync<AdminUserOverview>(
                @"usp_get_admin_users_overview",
                (object)parameters,
                commandType: CommandType.StoredProcedure
            );
            return adminUsers as IReadOnlyCollection<AdminUserOverview>;
        }
    }
}
