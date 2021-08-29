using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Util;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Service.Validators.Admin;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Models.Admin.Output;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Services.Admin
{
    public interface IRoleService
    {
        Task<Result<FetchRolePermissionOutputModel>> GetRolePermissionsById(int roleId);
        Task<Result<FetchRoleOutputModel>> GetRoleById(int roleId);
        Task<AddResult<CreateRoleOutputModel>> CreateRole(CreateRoleInputModel model);
        Task<Result<MessageOutputModel>> ResetRolePermissions(int roleId, int[] permissions);
        Task<Result<MessageOutputModel>> UpdateRoleStatus(UpdateRoleStatusInputModel model);
        Task<Result<FetchRolesOutputModel>> GetRoles();
        Task<Result<MessageOutputModel>> UpdateRole(UpdateRoleInputModel model);
        Task<Result<MessageOutputModel>> DeleteRole(long id);
    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleValidator _roleValidator;
        private readonly IPermissionService _permissionService;
        public RoleService(IRoleRepository roleRepository, IRoleValidator roleValidator, IPermissionService permissionService)
        {
            _roleRepository = roleRepository;
            _roleValidator = roleValidator;
            _permissionService = permissionService;
        }

        public async Task<AddResult<CreateRoleOutputModel>> CreateRole(CreateRoleInputModel model)
        {
            var validationResult = _roleValidator.ValidateNewRole(model);
            var existingRole = await _roleRepository.FindByRoleName(model.RoleName);
            if (validationResult.IsValid && existingRole is null)
            {
                var role = model.ToRole();
                role.Id = await _roleRepository.Create(role);
                return new AddResult<CreateRoleOutputModel>(validationResult, false, CreateRoleOutputModel.FromRole(role));
            }
            return new AddResult<CreateRoleOutputModel>(validationResult, validationResult.IsValid, null);
        }

        public async Task<Result<MessageOutputModel>> DeleteRole(long id)
        {
            var result = await _roleRepository.DeleteTransaction(id);
            var results = result.Split(':');
            var (statusCode, responseCode) = Helper.MapDbResponseCodeToStatusCode(results[0]);
            if (!responseCode.Equals(ResponseCodes.Success))
                throw new ServiceException(responseCode, results[1].Trim(), statusCode);
            return new Result<MessageOutputModel>(new ValidationResult(), MessageOutputModel.FromStringMessage("Role deleted successfully"));
        }

        public async Task<Result<FetchRoleOutputModel>> GetRoleById(int roleId)
        {
            var role = await _roleRepository.FindById(roleId);
            _ = role ?? throw new ServiceException(ResponseCodes.NotFound, "Role not found", 404);
            return new Result<FetchRoleOutputModel>(new ValidationResult(), FetchRoleOutputModel.FromRole(role));
        }

        public async Task<Result<FetchRolePermissionOutputModel>> GetRolePermissionsById(int roleId)
        {
            var role = await _roleRepository.FindById(roleId);
            _ = role ?? throw new ServiceException(ResponseCodes.NotFound, "Role not found", 404);
            var permissions = await _roleRepository.GetPermissionsByRoleId(roleId);
            role.WithPermissions(permissions);
            return new Result<FetchRolePermissionOutputModel>(new ValidationResult(), FetchRolePermissionOutputModel.FromRolePermission(role));
        }

        public async Task<Result<FetchRolesOutputModel>> GetRoles()
        {
            var roles = await _roleRepository.Fetch();
            return new Result<FetchRolesOutputModel>(new ValidationResult(), FetchRolesOutputModel.FromRoles(roles));
        }

        public async Task<Result<MessageOutputModel>> ResetRolePermissions(int roleId, int[] permissions)
        {
            var validationResult = _roleValidator.ValidateResetRolePermission(roleId, permissions);
            if (validationResult.IsValid)
            {
                var role = await _roleRepository.FindById(roleId);
                _ = role ?? throw new ServiceException(ResponseCodes.NotFound, "Role not found", 404);
                if (!role.Status) throw new ServiceException(ResponseCodes.InvalidRequest, "Role not approved", 400);
                if (permissions.HasDuplicate()) throw new ServiceException(ResponseCodes.InvalidRequest, "Permissions must be unique", 400);
                var invalidPermissions = await PermissionsNotExist(permissions);
                if (invalidPermissions.Count > 0) throw new ServiceException(ResponseCodes.InvalidRequest, $"'{string.Join(",", invalidPermissions.Keys)}' not found or has not been approved", 404);
                await _roleRepository.ResetRolePermissions(roleId, permissions);
                return new Result<MessageOutputModel>(validationResult, MessageOutputModel.FromStringMessage("Role permission(s) updated successfully"));
            }
            return new Result<MessageOutputModel>(validationResult, null);
        }

        public async Task<Result<MessageOutputModel>> UpdateRole(UpdateRoleInputModel model)
        {
            var validationResult = _roleValidator.ValidateUpdateRole(model);
            if(validationResult.IsValid)
            {
                var role = await _roleRepository.FindById(model.Id);
                _ = role ?? throw new ServiceException(ResponseCodes.NotFound, "Role not found", 404);
                if (!model.RoleName.Equals(role.RoleName, StringComparison.InvariantCultureIgnoreCase))
                {
                    var existingRole = await _roleRepository.FindByRoleName(model.RoleName);
                    if (existingRole is not null)
                        throw new ServiceException(ResponseCodes.Conflict, "Role exist", 409);
                    var newRole = model.ToRole();
                    await _roleRepository.Update(role.Id, newRole);
                }
                return new Result<MessageOutputModel>(validationResult, MessageOutputModel.FromStringMessage("Role updated."));
            }
            return new Result<MessageOutputModel>(validationResult, null);
        }

        public async Task<Result<MessageOutputModel>> UpdateRoleStatus(UpdateRoleStatusInputModel model)
        {
            var validationResult = _roleValidator.ValidateUpdateRoleStatus(model);
            if (validationResult.IsValid)
            {
                var role = await _roleRepository.FindById(model.RoleId);
                _ = role ?? throw new ServiceException(ResponseCodes.NotFound, "Role not found", 404);
                if (!role.Status.Equals(model.Status))
                {
                    var roleModel = model.ToRole();
                    await _roleRepository.UpdateStatus(roleModel);
                }
            }
            return new Result<MessageOutputModel>(validationResult, MessageOutputModel.FromStringMessage("Role status has been updated."));
        }

        private async Task<Dictionary<int, bool>> PermissionsNotExist(int[] permissions)
        {
            var validationResult = new Dictionary<int, bool>();
            foreach(var permission in permissions)
            {
                var exist = await _permissionService.PermissionExist(permission);
                if (!exist) validationResult.Add(permission, true);
            }
            return validationResult;
        }
    }
}
