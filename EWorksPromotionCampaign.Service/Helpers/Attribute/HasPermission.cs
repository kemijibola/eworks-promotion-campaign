using EWorksPromotionCampaign.Service.Services.Admin;
using EWorksPromotionCampaign.Shared.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IAdminUserService = EWorksPromotionCampaign.Service.Services.Admin.IUserService;

namespace EWorksPromotionCampaign.Service.Helpers.Attribute
{
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(string claim) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[]
            {
                new Claim("Permission", claim)
            };
        }

        public class ClaimRequirementFilter : IAsyncAuthorizationFilter
        {

            private readonly Claim _claim;
            private readonly ILogger<ClaimRequirementFilter> _logger;
            private readonly IAdminUserService _adminUserService;
            private readonly IRoleService _roleService;

            public ClaimRequirementFilter(Claim claim, ILogger<ClaimRequirementFilter> logger, IAdminUserService adminUserService, IRoleService roleService)
            {
                _logger = logger;
                _claim = claim;
                _roleService = roleService;
                _adminUserService = adminUserService;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                try
                {
                    var userName = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    var user = await _adminUserService.GetByEmail(userName);
                    if (user.Data is null)
                    {
                        _logger.LogInformation($"user {userName} does not exist");
                        context.Result = new CustomResponseResult(ResponseCodes.NotFound, "Unauthorized", StatusCodes.Status401Unauthorized);
                        return;
                    }

                    if (!user.Data.Status || user.Data.IsDeactivated)
                    {
                        _logger.LogInformation($"user {userName} is deactivated");
                        context.Result = new CustomResponseResult(ResponseCodes.Forbidden,
                            "User is deactivated, please contact administrator", StatusCodes.Status403Forbidden);
                        return;
                    }

                    var userRole = await _roleService.GetRolePermissionsById(user.Data.RoleId);
                    if (!userRole.Data.Status)
                    {
                        context.Result = new CustomResponseResult(ResponseCodes.Forbidden, "Role not activated",
                            StatusCodes.Status403Forbidden);
                        return;
                    }
                    var permissions = userRole.Data.Permissions;
                    
                    if (permissions == null || !permissions.Any(x => x.PermissionName.Equals(_claim.Value)))
                    {
                        context.Result = new CustomResponseResult(ResponseCodes.Forbidden, "No access to this resource",
                            StatusCodes.Status403Forbidden);
                        return;
                    }
                    var requiredPermission = permissions.Where(x => x.PermissionName.Equals(_claim.Value));
                    if (requiredPermission.Any(x => !x.Status))
                    {
                        context.Result = new CustomResponseResult(ResponseCodes.Forbidden, "No access to this resource",
                            StatusCodes.Status403Forbidden);
                        return;
                    }
                    var claims = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("User", JsonConvert.SerializeObject(user))
                    });

                    context.HttpContext.User.AddIdentity(claims);
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Unexpected error occured{exception.Message}:{exception.StackTrace}");
                    context.Result = new CustomResponseResult(ResponseCodes.UnexpectedError,
                        "An unexpected error occured", StatusCodes.Status400BadRequest);
                }
            }
        }
        public class CustomResponseResult : JsonResult
        {
            public CustomResponseResult(string responseCode, string responseDescription, int customStatusCode) : base(
                new CustomErrorResponse(responseCode, responseDescription))
            {
                StatusCode = customStatusCode;
            }
        }

        public class CustomErrorResponse
        {
            public string ResponseCode { get; }
            public string ResponseDescription { get; }


            public CustomErrorResponse(string responseCode, string responseDescription)
            {
                ResponseCode = responseCode;
                ResponseDescription = responseDescription;
            }
        }
    }
}
