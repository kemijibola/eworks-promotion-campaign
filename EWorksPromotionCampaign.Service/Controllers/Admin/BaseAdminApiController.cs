using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using AdminUser = EWorksPromotionCampaign.Shared.Models.Admin.Domain.User;

namespace EWorksPromotionCampaign.Service.Controllers.Admin
{
    public class BaseAdminApiController : ControllerBase
    {
        protected AdminUser GetAuthUser()
        {
            return JsonConvert.DeserializeObject<AdminUser>(User.Claims.FirstOrDefault(c => c.Type == "User")?.Value);
        }
    }
}
