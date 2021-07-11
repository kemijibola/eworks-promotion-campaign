using EWorksPromotionCampaign.Service.Helpers.Attribute;
using EWorksPromotionCampaign.Service.Services.Admin;
using EWorksPromotionCampaign.Service.Util;
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

namespace EWorksPromotionCampaign.Service.Controllers.Admin
{
    [Authorize]
    [Route("api/v1/eadmin/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ILogger<CampaignsController> _logger;
        private readonly ICampaignRewardService _campaignRewardService;
        private readonly ICampaignService _campaignService;
        public CampaignsController(ILogger<CampaignsController> logger, ICampaignRewardService campaignRewardService, ICampaignService campaignService)
        {
            _logger = logger;
            _campaignRewardService = campaignRewardService;
            _campaignService = campaignService;
        }

        [HasPermission(Permission.CanCreateCampaignReward)]
        [HttpPost("rewards")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] CreateCampaignRewardInputModel requestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = Helper.StringifyValidationErrors(ModelState)
                    });
                var result = await _campaignRewardService.AddCampaignReward(requestModel);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status201Created, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to create campaign reward: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanCreateCampaign)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] CreateCampaignInputModel requestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = Helper.StringifyValidationErrors(ModelState)
                    });
                var addResult = await _campaignService.CreateCampaign(requestModel);
                if (addResult.IsSuccess)
                    return StatusCode(StatusCodes.Status201Created, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", addResult.Data });
                if (addResult.IsDuplicate)
                    return StatusCode(StatusCodes.Status409Conflict);
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = addResult.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to create campaign: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanEditCampaign)]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditCampaign(long id, [FromBody] EditCampaignInputModel requestModel)
        {
            try
            {
                requestModel.Id = id;
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        ResponseCode = ResponseCodes.InvalidRequest,
                        ResponseDescription = Helper.StringifyValidationErrors(ModelState)
                    });
                var result = await _campaignService.EditCampaign(requestModel);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to edit campaign: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanViewCampaign)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id, [FromQuery]bool with_campaign_rewards)
        {
            try
            {
                var result = await _campaignService.GetCampaignById(id, with_campaign_rewards);
                if (result.IsSuccess)
                    return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to fetch campaign: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanStartCampaign)]
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StartOrPauseCampaign(long id, [FromForm] string action)
        {
            try
            {
                var actionDictionary = new Dictionary<string, string>
                {
                    { "start", "start"},
                    { "pause", "pause"}
                };
                if (actionDictionary.ContainsKey(action))
                {
                    var result = action.Equals("start", StringComparison.InvariantCultureIgnoreCase) ? await _campaignService.StartCampaign(id) : await _campaignService.PauseCampaign(id);
                    if (result.IsSuccess)
                        return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                    return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
                }
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = "Invalid action" });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to start/pause campaign: {sEx.Message} {sEx.StackTrace}");
                return StatusCode(sEx.StatusCode, new ServiceResponse(sEx.ResponseCode, sEx.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occured: {ex.Message} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { ResponseCode = ResponseCodes.UnexpectedError, ResponseDescripion = "An unexpected error occured. Please try again!" });
            }
        }

        [HasPermission(Permission.CanPauseCampaign)]
        [HttpPost("rewards/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StartOrPauseCampaignReward(long id, [FromForm] string action)
        {
            try
            {
                var actionDictionary = new Dictionary<string, string>
                {
                    { "start", "start"},
                    { "pause", "pause"}
                };
                if (actionDictionary.ContainsKey(action))
                {
                    var result = action.Equals("start", StringComparison.InvariantCultureIgnoreCase) ? await _campaignRewardService.StartCampaignReward(id) : await _campaignRewardService.PauseCampaignReward(id);
                    if (result.IsSuccess)
                        return StatusCode(StatusCodes.Status200OK, new { ResponseCode = ResponseCodes.Success, ResponseDescripion = "Success", result.Data });
                    return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = result.Description });
                }
                return BadRequest(new { responseCode = ResponseCodes.InvalidRequest, responseDescription = "Invalid action" });
            }
            catch (ServiceException sEx)
            {
                _logger.LogError($"Unable to start/pause campaign reward: {sEx.Message} {sEx.StackTrace}");
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
