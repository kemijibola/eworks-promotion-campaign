using EWorksPromotionCampaign.Service.Services;
using EWorksPromotionCampaign.Service.Util;
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
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IUserService _userService;
        public RegisterController(ILogger<RegisterController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] RegisterInputModel requestModel)
        {
            try
            {
                var addResult = await _userService.AddUser(requestModel);
                if (addResult.IsSuccess)
                    return StatusCode(StatusCodes.Status201Created, new {ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", addResult.Data });
                if (addResult.IsDuplicate)
                    return StatusCode(StatusCodes.Status409Conflict);
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = addResult.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to register user: {sEx.Message} {sEx.StackTrace}");
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
