using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Security;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models.Domain;
using EWorksPromotionCampaign.Shared.Models.Input;
using EWorksPromotionCampaign.Shared.Models.Output;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Services
{
    public interface IUserService
    {
        Task<AddResult<RegisterOutputModel>> AddUser(RegisterInputModel model);
        Task<Result<LoginOutputModel>> Authenticate(LoginInputModel model);
        Task<Result<FetchUserByEmailOutputModel>> GetUserByEmail(string email);
    }
    public class UserService : IUserService
    {
        private readonly IUserValidator _userValidator;
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly ILoginValidator _loginValidator;
        public UserService(IUserValidator userValidator,
            IUserRepository userRepository, IOptions<AppSettings> appSettings, ILoginValidator loginValidator)
        {

            _userValidator = userValidator;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _loginValidator = loginValidator;
        }

        public async Task<AddResult<RegisterOutputModel>> AddUser(RegisterInputModel model)
        {
            var validationResult = _userValidator.ValidateNewUser(model);
            var existingEmailOrPhone = await _userRepository.GetUserByEmailOrPhone(model.Email, model.Phone);
            if (validationResult.IsValid && existingEmailOrPhone is null)
            {
                var user = model.ToUser();
                var salt = PasswordSalt.Create();
                var hash = PasswordHash.Create(model.Password, salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                user.IsPhoneVerified = false;
                user.IsEmailVerified = false;
                user.Id = await _userRepository.Create(user);

                var authData = user.GenerateToken(new JwtHandler(_appSettings));

                return new AddResult<RegisterOutputModel>(validationResult, false, RegisterOutputModel.FromUser(user, authData.Token));
            }
            return new AddResult<RegisterOutputModel>(validationResult, validationResult.IsValid, null);
        }
        public async Task<Result<LoginOutputModel>> Authenticate(LoginInputModel model)
        {
            var validationResult = _loginValidator.ValidateLogin(model);
            if (validationResult.IsValid)
            {
                var identifier = string.IsNullOrEmpty(model.Email) ? model.Phone : model.Email;
                var user = await _userRepository.GetUserByIdentifier(identifier);
                _ = user ?? throw new ServiceException(ResponseCodes.NotFound, "Invalid username/password", 404);
                var isValidated = PasswordHash.Validate(model.Password, user.PasswordSalt, user.PasswordHash);
                if (!isValidated)
                    return null;
                var authData = user.GenerateToken(new JwtHandler(_appSettings));
                return new Result<LoginOutputModel>(validationResult, LoginOutputModel.FromUser(user, authData.Token));
            }
            return new Result<LoginOutputModel>(validationResult, null);
        }

        public async Task<Result<FetchUserByEmailOutputModel>> GetUserByEmail(string email)
        {
            var validationResult = new ValidationResult();
            var existingUser = await _userRepository.GetUserByEmail(email);
            if (existingUser is null)
                return new Result<FetchUserByEmailOutputModel>(validationResult, null);
            return new Result<FetchUserByEmailOutputModel>(validationResult, FetchUserByEmailOutputModel.FromUser(existingUser));
        }
        //public async Task<Result<UpdateUserOutputModel>> UpdateUser(UpdateUserInputModel model)
        //{
        //    var validationResult = _userValidator.ValidateExistingUser(model);
        //    var existingUser = await _userRepository.FindById(model.Id);
        //    _ = existingUser ?? throw new ServiceException("Not Found", "User not found", 404);
        //    if (validationResult.IsValid)
        //    {
        //        var validateExistingEmailOrPhone = false;
        //        var user = model.ToUser();
        //        if (!string.IsNullOrEmpty(model.Phone))
        //        {
        //            if (!user.Phone.Equals(existingUser.Phone))
        //            {
        //                user.IsPhoneVerified = false;
        //                validateExistingEmailOrPhone = true;
        //            }
        //        }
        //        else
        //            user.IsPhoneVerified = existingUser.IsPhoneVerified;

        //        if (!string.IsNullOrEmpty(model.Email))
        //        {
        //            if (!user.Email.Equals(existingUser.Email))
        //            {
        //                user.IsEmailVerified = false;
        //                validateExistingEmailOrPhone = true;
        //            }
        //        }
        //        else
        //            user.IsEmailVerified = existingUser.IsEmailVerified;

        //        if (validateExistingEmailOrPhone)
        //        {
        //            var existingEmailOrPhone = await _userRepository.GetUserByEmailOrPhone(user.Email, user.Phone);
        //            if (existingEmailOrPhone != null)
        //                throw new ServiceException("Duplicate", "Email/Phone already exist", 409);
        //        }

        //        await _userRepository.Update(user.Id, user);

        //        user.Bvn = existingUser.Bvn;
        //        user.BvnPhone = existingUser.BvnPhone;
        //        user.IsBvnVerified = existingUser.IsBvnVerified;
        //        user.IsDeactivated = existingUser.IsDeactivated;
        //        user.IsDisabled = existingUser.IsDisabled;
        //        user.LockedOutEnabled = existingUser.LockedOutEnabled;
        //        user.LockedOut = existingUser.LockedOut;
        //        user.AccessFailedCount = existingUser.AccessFailedCount;
        //        user.DisabledComment = existingUser.DisabledComment;
        //        user.DisabledAt = existingUser.DisabledAt;
        //        user.CreatedAt = existingUser.CreatedAt;
        //        user.LockedOutAt = existingUser.LockedOutAt;
        //        user.UpdatedAt = existingUser.UpdatedAt;

        //        return new Result<UpdateUserOutputModel>(validationResult, UpdateUserOutputModel.FromUser(user));
        //    }
        //    return new Result<UpdateUserOutputModel>(validationResult, null);
        //}
    }
} 
