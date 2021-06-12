﻿using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Service.Validators.Admin;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Models.Admin.Output;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Services.Admin
{
    public interface IPermissionService
    {
        Task<Result<FetchPermissionOutputModel>> GetPermissionById(int permissionId);
        Task<bool> PermissionExist(int permissionId);
        Task<Result<string>> UpdateRoleStatus(UpdatePermissionStatusInputModel model);
        Task<Result<FetchPermissionsOutputModel>> GetPermissions();
    }
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionValidator _permissionValidator;
        public PermissionService(IPermissionRepository permissionRepository, IPermissionValidator permissionValidator)
        {
            _permissionRepository = permissionRepository;
            _permissionValidator = permissionValidator;
        }
        public async Task<Result<FetchPermissionOutputModel>> GetPermissionById(int permissionId)
        {
            var permission = await _permissionRepository.FindById(permissionId);
            _ = permission ?? throw new ServiceException(ResponseCodes.NotFound, "Permission not found", 404);
            return new Result<FetchPermissionOutputModel>(new ValidationResult(), FetchPermissionOutputModel.FromPermission(permission));
        }

        public async Task<Result<FetchPermissionsOutputModel>> GetPermissions()
        {
            var permissions = await _permissionRepository.Fetch();
            return new Result<FetchPermissionsOutputModel>(new ValidationResult(), FetchPermissionsOutputModel.FromPermissions(permissions));
        }

        public async Task<bool> PermissionExist(int permissionId)
        {
            var permission = await _permissionRepository.FindById(permissionId);
            if (permission is null || !permission.Status)
                return false;
            return true;       
        }

        public async Task<Result<string>> UpdateRoleStatus(UpdatePermissionStatusInputModel model)
        {
            var validationResult = _permissionValidator.ValidateUpdatePermissionStatus(model);
            if (validationResult.IsValid)
            {
                var permission = await _permissionRepository.FindById(model.PermissionId);
                _ = permission ?? throw new ServiceException(ResponseCodes.NotFound, "Permission not found", 404);
                if (!permission.Status.Equals(model.Status))
                {
                    var permissionModel = model.ToPermission();
                    await _permissionRepository.UpdateStatus(permissionModel);
                }
            }
            return new Result<string>(validationResult, "Permission status has been updated.");
        }
    }
}
