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
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Controllers.Admin
{
    [Authorize]
    [Route("api/v1/eadmin/[controller]")]
    [ApiController]
    public class RolesController : BaseAdminApiController
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRoleService _roleService;
        public RolesController(ILogger<RolesController> logger, IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }

        [HasPermission(Permission.CanCreateAdminRole)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] CreateRoleInputModel requestModel)
        {
            try
            {
                var addResult = await _roleService.CreateRole(requestModel);
                if (addResult.IsSuccess)
                    return StatusCode(StatusCodes.Status201Created, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", addResult.Data });
                if (addResult.IsDuplicate)
                    return StatusCode(StatusCodes.Status409Conflict);
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = addResult.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to create role: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanUpdateAdminRoleStatus)]
        [HttpPut("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRoleStatus([FromBody] UpdateRoleStatusInputModel requestModel)
        {
            try
            {
                requestModel.UserId = GetAuthUser().Id;
                var result = await _roleService.UpdateRoleStatus(requestModel);
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

        [HasPermission(Permission.CanAssignPermissionsToRole)]
        [HttpPut("{id}/permissions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRolePermissions(int id, int[] permissions)
        {
            try
            {
                var result = await _roleService.ResetRolePermissions(id, permissions);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to assign permissions to role: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanViewAdminRole)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _roleService.GetRoleById(id);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to fetch role: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanViewAdminRoles)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _roleService.GetRoles();
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to fetch roles: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanUpdateAdminRoleStatus)]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRole(long id, [FromBody] UpdateRoleInputModel requestModel)
        {
            try
            {
                requestModel.Id = id;
                var result = await _roleService.UpdateRole(requestModel);
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

        [HasPermission(Permission.CanCreateAdminRole)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRole(long id)
        {
            try
            {
                if (id < 1)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = "Provide a valid role id"
                    });
                var result = await _roleService.DeleteRole(id);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to delete role: {sEx.Message} {sEx.StackTrace}");
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
