using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string responseCode, string message, int statusCode) : base(message)
        {
            ResponseCode = responseCode;
            StatusCode = statusCode;
        }
        public string ResponseCode { get; }
        public int StatusCode { get; }
    }
}
