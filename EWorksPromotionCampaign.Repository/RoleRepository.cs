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
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<IReadOnlyCollection<Permission>> GetPermissionsByRoleId(int roleId);
        Task ResetRolePermissions(int roleId, int[] permissionIds);
        Task UpdateStatus(Role role);
        Task<Role> FindByRoleName(string name);
    }
    public class RoleRepository : IRoleRepository
    {
        private readonly string _defaultConnectionString;
        private readonly IPermissionRepository _permissionRepository;
        public RoleRepository(IConfiguration configuration, IPermissionRepository permissionRepository)
        {
            _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
            _permissionRepository = permissionRepository;
        }
        public async Task<long> Create(Role newItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var roleId = await conn.QueryFirstOrDefaultAsync<int>(
                @"usp_create_new_admin_role",
                new
                {
                    role_name = newItem.RoleName,
                    role_description = newItem.RoleDescription
                },
                commandType: CommandType.StoredProcedure);

            return roleId;
        }

        public Task Delete<TItem>(TItem id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<Role>> Fetch()
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var roles = await connection.QueryAsync<Role>(
                @"usp_get_roles",
                commandType: CommandType.StoredProcedure
            );
            return roles as IReadOnlyCollection<Role>;
        }

        public async Task<Role> FindById<TItem>(TItem id)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return await connection.QueryFirstOrDefaultAsync<Role>(
                @"usp_fetch_role_by_id",
                new
                {
                    id
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task ResetRolePermissions(int roleId, int[] permissionIds)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                await DeleteRolePermissions(roleId, transaction, connection);

                foreach (var permissionId in permissionIds)
                    await AddPermissionsToRole(permissionId, roleId, transaction, connection);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task Update<TItem>(TItem id, Role updateItem)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_role", new
                {
                    role_id = id,
                    role_name = updateItem.RoleName,
                    role_description = updateItem.RoleDescription
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateStatus(Role role)
        {
            await using var conn = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await conn.ExecuteAsync(
                @"usp_update_role_status", new
                {
                    role_id = role.Id,
                    status = role.Status,
                    comment = role.StatusComment,
                    updated_by = role.StatusUpdatedBy
                },
                commandType: CommandType.StoredProcedure
            );
        }
        private static async Task DeleteRolePermissions(int roleId, IDbTransaction transaction, IDbConnection connection)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await connection.QueryAsync(@"usp_delete_role_permissions_by_role_id", new { role_id = roleId },
                commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        private static async Task AddPermissionsToRole(int permissionId, int roleId, IDbTransaction transaction,
            IDbConnection connection)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await connection.QueryAsync(@"usp_add_permission_to_role", new { permission_id = permissionId, role_id = roleId },
                commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public async Task<IReadOnlyCollection<Permission>> GetPermissionsByRoleId(int roleId)
        {
            return await _permissionRepository.FetchByRoleId(roleId);
        }

        public async Task<Role> FindByRoleName(string name)
        {
            await using var connection = new SqlConnection(_defaultConnectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var role = await connection.QueryFirstOrDefaultAsync<Role>(
                @"usp_fetch_role_by_role_name",
                new
                {
                    role_name = name
                },
                commandType: CommandType.StoredProcedure);
            return role;
        }
    }
}
