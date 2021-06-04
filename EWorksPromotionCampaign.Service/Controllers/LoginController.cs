using EWorksPromotionCampaign.Service.Services;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models.Input;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userService;
        public LoginController(ILogger<LoginController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] LoginInputModel requestModel)
        {
            try
            {
                var loginResult = await _userService.Authenticate(requestModel);
                if (loginResult == null)
                    return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = "Authentication failed.Check your credentials and try again!" });
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
