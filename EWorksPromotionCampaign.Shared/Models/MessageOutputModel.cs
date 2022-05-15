using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models
{
    public class MessageOutputModel
    {
        private MessageOutputModel(string message)
        {
            Message = message;
        }
        public string Message { get; set; }

        public static MessageOutputModel FromStringMessage(string message)
        {
            return new MessageOutputModel(message);
        }
    }
}
