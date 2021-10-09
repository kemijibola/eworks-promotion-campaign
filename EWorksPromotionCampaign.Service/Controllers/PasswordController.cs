using EWorksPromotionCampaign.Service.Services;
using EWorksPromotionCampaign.Service.Util;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models.Input.Account;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly ILogger<PasswordController> _logger;
        private readonly IPasswordService _passwordService;
        public PasswordController(ILogger<PasswordController> logger, IPasswordService passwordService)
        {
            _logger = logger;
            _passwordService = passwordService;
        }

        [AllowAnonymous]
        [HttpPost("forgot", Name = "ForgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ForgotPassword([FromBody] Shared.Models.Input.ForgotPasswordInputModel requestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = Helper.StringifyValidationErrors(ModelState)
                    });
                var forgotPasswordResult = await _passwordService.ForgotPassword(requestModel);
                if (forgotPasswordResult.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { data = forgotPasswordResult.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = forgotPasswordResult.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Forgot password request failed: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [AllowAnonymous]
        [HttpPost("reset", Name = "ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordInputModel requestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = Helper.StringifyValidationErrors(ModelState)
                    });
                var resetPasswordResult = await _passwordService.ResetPassword(requestModel);
                if (resetPasswordResult.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { data = resetPasswordResult.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = resetPasswordResult.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Reset password request failed: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }
    }
}
