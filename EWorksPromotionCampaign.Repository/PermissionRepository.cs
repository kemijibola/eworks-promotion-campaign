using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace EWorksPromotionCampaign.Repository
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<IReadOnlyCollection<Permission>> FetchByRoleId(long roleId);
        Task UpdateStatus(Permission permission);
        Task<Permission> FindByPermissionName(string name);
    }
    public class PermissionRepository : IPermissionRepository
    {
        private readonly string _defaultConnectionString;
        public PermissionRepository(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        }
        public async Task<long> Create(Permission newItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var permissionId = await conn.QueryFirstOrDefaultAsync<int>(
                @"usp_create_new_admin_permission",
                new
                {
                    permission_name = newItem.PermissionName,
                    permission_description = newItem.PermissionDescription
                },
                commandType: CommandType.StoredProcedure);

            return permissionId;
        }

        public Task Delete<TItem>(TItem id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<Permission>> Fetch()
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var permissions = await connection.QueryAsync<Permission>(
                @"usp_get_permissions",
                commandType: CommandType.StoredProcedure
            );
            return permissions as IReadOnlyCollection<Permission>;
        }

        public async Task<IReadOnlyCollection<Permission>> FetchByRoleId(long roleId)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var permissions = await connection.QueryAsync<Permission>(
                @"usp_get_role_permissions_by_role_id",
                new
                {
                    role_id = roleId
                },
                commandType: CommandType.StoredProcedure
            );
            return permissions as IReadOnlyCollection<Permission>;
        }

        public async Task<Permission> FindById<TItem>(TItem id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return await connection.QueryFirstOrDefaultAsync<Permission>(
                @"usp_fetch_permission_by_id",
                new
                {
                    id
                },
                commandType: CommandType.StoredProcedure);
        }

        public Task Update<TItem>(TItem id, Permission updateItem)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateStatus(Permission permission)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_permission_status", new
                {
                    permission_id = permission.Id,
                    status = permission.Status,
                    comment = permission.StatusComment,
                    updated_by = permission.StatusUpdatedBy
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<Permission> FindByPermissionName(string name)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return await connection.QueryFirstOrDefaultAsync<Permission>(
                @"usp_fetch_permission_by_permission_name",
                new
                {
                    permission_name = name
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
