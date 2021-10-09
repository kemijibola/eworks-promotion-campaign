using EWorksPromotionCampaign.Shared.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected User GetAuthUser()
        {
            return JsonConvert.DeserializeObject<User>(User.Claims.FirstOrDefault(c => c.Type == "User")?.Value);
        }
    }
}
