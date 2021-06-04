using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Util
{
    public class ServiceResponse
    {
        public ServiceResponse(string responseCode, string responseDescription)
        {
            ResponseCode = responseCode;
            ResponseDescription = responseDescription;
        }
        public ServiceResponse(string responseDescription)
        {
            ResponseDescription = responseDescription;
        }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
    }
}
