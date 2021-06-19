using EWorksPromotionCampaign.Shared.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class UpdateConfigurationInputModel
    {
        public long Id { get; set; }
        public string ConfigurationKey { get; set; }
        public string ConfigurationValue { get; set; }
        public Configuration ToConfiguration()
        {
            var config = new Configuration(Id)
            {
                Key = ConfigurationKey,
                Value = ConfigurationValue
            };
            return config;
        }
    }
}
