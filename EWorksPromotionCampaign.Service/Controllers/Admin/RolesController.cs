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
using AdminUser = EWorksPromotionCampaign.Shared.Models.Admin.Domain.User;

namespace EWorksPromotionCampaign.Service.Controllers.Admin
{
    [Authorize]
    [Route("api/v1/eadmin/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
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

        private AdminUser GetAuthUser()
        {
            return JsonConvert.DeserializeObject<AdminUser>(User.Claims.FirstOrDefault(c => c.Type == "User")?.Value);
        }
    }
}
