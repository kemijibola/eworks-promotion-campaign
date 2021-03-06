using EWorksPromotionCampaign.Service.Helpers.Attribute;
using EWorksPromotionCampaign.Service.Services.Admin;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Controllers.Admin
{
    [Authorize]
    [Route("api/v1/eadmin/[controller]")]
    [ApiController]
    public class PermissionsController : BaseAdminApiController
    {
        private readonly ILogger<PermissionsController> _logger;
        private readonly IPermissionService _permissionService;
        public PermissionsController(ILogger<PermissionsController> logger, IPermissionService permissionService)
        {
            _logger = logger;
            _permissionService = permissionService;
        }

        [HasPermission(Permission.CanViewPermissions)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _permissionService.GetPermissions();
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to fetch permissions: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _permissionService.GetPermissionById(id);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to fetch permissions: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        //[HasPermission(Permission.CanCreatePermission)]
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //public async Task<IActionResult> Post([FromBody] CreatePermissionInputModel requestModel)
        //{
        //    try
        //    {
        //        var addResult = await _permissionService.CreatePermission(requestModel);
        //        if (addResult.IsSuccess)
        //            return StatusCode(StatusCodes.Status201Created, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", addResult.Data });
        //        if (addResult.IsDuplicate)
        //            return StatusCode(StatusCodes.Status409Conflict);
        //        return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = addResult.Description });
        //    }
        //    catch (ServiceException sEx)
        //    {
        //        _logger.LogError($"Unable to create permission: {sEx.Message} {sEx.StackTrace}");
        //        return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
        //    }
        //}

        [HasPermission(Permission.CanUpdateAdminPermissionStatus)]
        [HttpPut("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPermissionStatus([FromBody] UpdatePermissionStatusInputModel requestModel)
        {
            try
            {
                requestModel.UserId = GetAuthUser().Id;
                var result = await _permissionService.UpdatePermissionStatus(requestModel);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to update role status: {sEx.Message} {sEx.StackTrace}");
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
