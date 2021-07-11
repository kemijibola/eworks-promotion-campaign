using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Security;
using EWorksPromotionCampaign.Service.Util;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models;
using EWorksPromotionCampaign.Shared.Models.Input.Account;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Services
{
    public interface IPasswordService
    {
        Task<Result<MessageOutputModel>> ForgotPassword(ForgotPasswordInputModel model);
        Task<Result<MessageOutputModel>> ResetPassword(ResetPasswordInputModel model);
    }
    public class PasswordService : IPasswordService
    {
        private readonly ILogger<PasswordService> _logger;
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;
        private readonly IUserValidator _userValidator;
        private readonly IPasswordRepository _passwordRepository;
        private readonly ITwoFactorRepository _twoFactorRepository;
        private readonly TokenType _emailTokenType;
        public PasswordService(ILogger<PasswordService> logger, IOptions<AppSettings> appSettings,
            IUserService userService, IUserValidator userValidator, IPasswordRepository passwordRepository, ITwoFactorRepository twoFactorRepository)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _userService = userService;
            _userValidator = userValidator;
            _passwordRepository = passwordRepository;
            _emailTokenType = TokenType.jwt;
            _twoFactorRepository = twoFactorRepository;
        }
        public async Task<Result<MessageOutputModel>> ForgotPassword(ForgotPasswordInputModel model)
        {
            var validationResult = _userValidator.ValidateForgotPassword(model);
            var existingUser = await _userService.GetUserByEmail(model.Email);
            if (validationResult.IsValid && existingUser != null)
            {
                var tokenGenerator = TwoFactorAuthentication.Type(_emailTokenType);
                var (generatedToken, _, _) = tokenGenerator.GenerateToken(6, DigitType.alphanumeric);
                tokenGenerator.Requester = model.Email;
                tokenGenerator.Token = generatedToken;
                tokenGenerator.TypeOfToken = TokenType.jwt.ToString();
                tokenGenerator.RequestId = Guid.NewGuid().ToString();
                _ = await _twoFactorRepository.Create(tokenGenerator);

                //::TODO send password reset mail
            }
            return new Result<MessageOutputModel>(validationResult, MessageOutputModel.FromStringMessage("We will send a reset email if the email matches an account."));
        }

        public async Task<Result<MessageOutputModel>> ResetPassword(ResetPasswordInputModel model)
        {
            var validationResult = _userValidator.ValidateResetPassword(model);
            var passwordTokenRequest = await _twoFactorRepository.GetTokenReqestByToken(model.Token);
            _ = passwordTokenRequest ?? throw new ServiceException(ResponseCodes.NotFound, "Invalid password reset token", 404);
            var existingUser = await _userService.GetUserByEmail(passwordTokenRequest.Requester);
            _ = existingUser ?? throw new ServiceException(ResponseCodes.NotFound, "Invalid user", 404);
            if (validationResult.IsValid)
            {
                var tokenGenerator = TwoFactorAuthentication.Type(_emailTokenType);
                var tokenData = tokenGenerator.ExtractTokenData(passwordTokenRequest.Token);
                if (DateTime.UtcNow.CompareTo(tokenData.ValidTo.ToUniversalTime()) == 1)
                {
                    validationResult.Errors.Add(nameof(model.Token), "Password reset token has expired.");
                    return new Result<MessageOutputModel>(validationResult, null);
                }

                await ChangePassword(existingUser.Email, model.Password);
                await _twoFactorRepository.Delete(passwordTokenRequest.RequestId);
                return new Result<MessageOutputModel>(validationResult, MessageOutputModel.FromStringMessage("Password reset successful"));
            }
            return new Result<MessageOutputModel>(validationResult, null);
        }
        private async Task ChangePassword(string email, string password)
        {
            var salt = PasswordSalt.Create();
            var hash = PasswordHash.Create(password, salt);

            await _passwordRepository.UpdateUserPasswordData(email, hash, salt);
            // TODO:: send notification mail
        }
    }
}
