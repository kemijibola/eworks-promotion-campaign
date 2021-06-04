using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Util
{
    public class ResponseCodes
    {
        public static readonly string Success = "10200";
        public static readonly string InvalidRequest = "10400";
        public static readonly string UnexpectedError = "10500";
        public static readonly string Conflict = "10409";
        public static readonly string Forbidden = "10403";
        public static readonly string NotFound = "10404";
        public static readonly string Unauthorized = "10401";
    }
}
