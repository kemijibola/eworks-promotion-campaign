using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Security;
using EWorksPromotionCampaign.Service.Util;
using EWorksPromotionCampaign.Service.Validators.Admin;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models.Admin.Output;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Services.Admin
{
    public interface IUserService
    {
        Task<Result<AdminLoginOutputModel>> Authenticate(AdminLoginInputModel model);
        Task<AddResult<CreateAdminUserOutputModel>> CreateAdmin(CreateAdminUserInputModel model);
        Task<Result<UpdateAdminUserOutputModel>> UpdateAdmin(UpdateAdminUserInputModel model);
        Task<Result<FetchUserByEmailOutputModel>> GetByEmail(string email);
        Task<Result<string>> UpdateUserStatus(UpdateUserStatusInputModel model);
        Task<Result<string>> UpdateUserDisabledStatus(UpdateDisabledStatusInputModel model);
    }
    public class UserService : IUserService
    {
        private readonly IAdminUserValidator _adminUserValidator;
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly IRoleService _roleService;
        public UserService(IAdminUserValidator adminUserValidator, IUserRepository userRepository, 
            IOptions<AppSettings> appSettings, IRoleService roleService)
        {
            _adminUserValidator = adminUserValidator;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _roleService = roleService;
        }
        public async Task<Result<AdminLoginOutputModel>> Authenticate(AdminLoginInputModel model)
        {
            var validationResult = _adminUserValidator.ValidateAdminLogin(model);
            if (validationResult.IsValid)
            {
                var identifier = string.IsNullOrEmpty(model.Email) ? model.Phone : model.Email;
                var user = await _userRepository.GetAdminUserByIdetifier(identifier);
                _ = user ?? throw new ServiceException(ResponseCodes.NotFound, "User not found", 404);
                if (user.LockedOut || !user.Status || user.RoleId == 0 || user.IsDisabled)
                    return null;
                var isValidated = PasswordHash.Validate(model.Password, user.PasswordSalt, user.PasswordHash);
                if (!isValidated)
                    return null;
                var authData = user.GenerateToken(new JwtHandler(_appSettings));
                var userRole = await _roleService.GetRolePermissionsById(user.RoleId);
                user.IsAdminUser();
                var role = FetchRolePermissionOutputModel.ToRolePermission(userRole.Data);
                return new Result<AdminLoginOutputModel>(validationResult, AdminLoginOutputModel.FromUser(user, role, authData.Token));
            }
            return new Result<AdminLoginOutputModel>(validationResult, null);
        }

        public async Task<AddResult<CreateAdminUserOutputModel>> CreateAdmin(CreateAdminUserInputModel model)
        {
            var validationResult = _adminUserValidator.ValidateNewAdmin(model);
            var existingEmailOrPhone = await _userRepository.GetAdminUserByEmailOrPhone(model.Email, model.Phone);
            if (validationResult.IsValid && existingEmailOrPhone is null)
            {
                var adminRole = await _roleService.GetRoleById(model.RoleId);
                if (!adminRole.Data.Status)
                    throw new ServiceException(ResponseCodes.NotFound, "Role not approved", 400);
                if (adminRole.Data.RoleName.Equals(Constants.SuperAdminRole, StringComparison.InvariantCultureIgnoreCase))
                    throw new ServiceException(ResponseCodes.NotFound, "Role can not be assigned to user", 400);
                var defaultPassword = Helper.RandomString(10, DigitType.alphanumeric);
                var user = model.ToUser();
                var salt = PasswordSalt.Create();
                var hash = PasswordHash.Create(defaultPassword, salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                user.Id = await _userRepository.CreateAdminUser(user);
                user.IsAdminUser();
                //TODO:: send mail to created user
                return new AddResult<CreateAdminUserOutputModel>(validationResult, false, CreateAdminUserOutputModel.FromUser(user));
            }
            return new AddResult<CreateAdminUserOutputModel>(validationResult, validationResult.IsValid, null);
        }

        public async Task<Result<FetchUserByEmailOutputModel>> GetByEmail(string email)
        {
            var validationResult = _adminUserValidator.ValidateFetchAdminByEmail(email);
            if (validationResult.IsValid)
            {
                var existingUser = await _userRepository.GetAdminUserByEmail(email);
                if (existingUser is null)
                    return new Result<FetchUserByEmailOutputModel>(validationResult, null);
                return new Result<FetchUserByEmailOutputModel>(validationResult, FetchUserByEmailOutputModel.FromUser(existingUser));
            }
            return new Result<FetchUserByEmailOutputModel>(validationResult, null);
        }

        public async Task<Result<UpdateAdminUserOutputModel>> UpdateAdmin(UpdateAdminUserInputModel model)
        {
            var validationResult = _adminUserValidator.ValidateUpdateAdmin(model);
            if (validationResult.IsValid)
            {
                var existingUser = await _userRepository.FindAdminById(model.Id);
                _ = existingUser ?? throw new ServiceException(ResponseCodes.NotFound, "User not found", 404);
                var updatedUser = model.ToUser(existingUser);
                await _userRepository.UpdateAdminUser(updatedUser);
                updatedUser.RoleId = existingUser.RoleId;
                return new Result<UpdateAdminUserOutputModel>(validationResult, UpdateAdminUserOutputModel.FromUser(updatedUser));
            }
            return new Result<UpdateAdminUserOutputModel>(validationResult, null);
        }

        public async Task<Result<string>> UpdateUserDisabledStatus(UpdateDisabledStatusInputModel model)
        {
            var validationResult = _adminUserValidator.ValidateUserDisabledStatus(model);
            if (validationResult.IsValid)
            {
                var user = await _userRepository.FindAdminById(model.Id);
                _ = user ?? throw new ServiceException(ResponseCodes.NotFound, "User not found", 404);
                if (!user.IsDisabled.Equals(model.Disabled))
                {
                    var userModel = model.ToUser();
                    await _userRepository.UpdateAdminUserDisabledStatus(userModel);
                }
            }
            return new Result<string>(validationResult, "User Disabled status has been updated.");
        }

        public async Task<Result<string>> UpdateUserStatus(UpdateUserStatusInputModel model)
        {
            var validationResult = _adminUserValidator.ValidateUpdateUserStatus(model);
            if (validationResult.IsValid)
            {
                var user = await _userRepository.FindAdminById(model.Id);
                _ = user ?? throw new ServiceException(ResponseCodes.NotFound, "User not found", 404);
                if (!user.Status.Equals(model.Status))
                {
                    var userModel = model.ToUser();
                    await _userRepository.UpdateAdminUserStatus(userModel);
                }
            }
            return new Result<string>(validationResult, "User status has been updated.");
        }
    }
}
