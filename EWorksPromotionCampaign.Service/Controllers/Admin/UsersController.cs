using EWorksPromotionCampaign.Service.Helpers.Attribute;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAdminUserService = EWorksPromotionCampaign.Service.Services.Admin.IUserService;
using Newtonsoft.Json;

namespace EWorksPromotionCampaign.Service.Controllers.Admin
{
    [Authorize]
    [Route("api/v1/eadmin/[controller]")]
    [ApiController]
    public class UsersController : BaseAdminApiController
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IAdminUserService _adminUserService;
        public UsersController(ILogger<UsersController> logger, IAdminUserService adminUserService)
        {
            _logger = logger;
            _adminUserService = adminUserService;
        }

        [HasPermission(Permission.CanCreateAdminUser)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] CreateAdminUserInputModel requestModel)
        {
            try
            {
                var addResult = await _adminUserService.CreateAdmin(requestModel);
                if (addResult.IsSuccess)
                    return StatusCode(StatusCodes.Status201Created, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", addResult.Data });
                if (addResult.IsDuplicate)
                    return StatusCode(StatusCodes.Status409Conflict);
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = addResult.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to create user: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanUpdateAdminUserStatus)]
        [HttpPut("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserStatus([FromBody] UpdateUserStatusInputModel requestModel)
        {
            try
            {
                requestModel.UserId = GetAuthUser().Id;
                var result = await _adminUserService.UpdateUserStatus(requestModel);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to update user status: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanEditAdminUser)]
        [HttpPut("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUser([FromBody] UpdateAdminUserInputModel requestModel)
        {
            try
            {
                var result = await _adminUserService.UpdateAdmin(requestModel);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to update user profile: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanToggleAdminUserDisabledStatus)]
        [HttpPut("disable/toggle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutToggleDisabledStatus([FromBody] UpdateDisabledStatusInputModel requestModel)
        {
            try
            {
                requestModel.UserId = GetAuthUser().Id;
                var result = await _adminUserService.UpdateUserDisabledStatus(requestModel);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to update user disabled status: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanViewAdminUsers)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, int pageSize = 5,
            string searchText = "")
        {
            try
            {
                var result = await _adminUserService.FetchAdminUsers(pageNumber, pageSize, searchText);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }
    }
}
