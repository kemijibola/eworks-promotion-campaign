using IAdminUserService = EWorksPromotionCampaign.Service.Services.Admin.IUserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EWorksPromotionCampaign.Shared.Exceptions;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Util;

namespace EWorksPromotionCampaign.Service.Controllers.Admin
{
    [Route("api/v1/eadmin/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IAdminUserService _adminUserService;
        public LoginController(ILogger<LoginController> logger, IAdminUserService adminUserService)
        {
            _logger = logger;
            _adminUserService = adminUserService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] AdminLoginInputModel requestModel)
        {
            try
            {
                var loginResult = await _adminUserService.Authenticate(requestModel);
                if (loginResult == null)
                    return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = "Login failed.Check your credentials and try again!" });
                if (loginResult.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { data = loginResult.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = loginResult.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to login user: {sEx.Message} {sEx.StackTrace}");
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
