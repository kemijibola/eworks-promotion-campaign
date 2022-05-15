using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Domain
{
    public class Configuration
    {
        public Configuration(long id)
        {
            Id = id;
        }
        public Configuration() {}
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
