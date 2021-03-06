using EWorksPromotionCampaign.Shared.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class CreateConfigurationInputModel
    {
        [Required] public string ConfigurationKey { get; set; }
        [Required] public string ConfigurationValue { get; set; }

        public Configuration ToConfiguration()
        {
            var config = new Configuration()
            {
                Key = ConfigurationKey,
                Value = ConfigurationValue
            };
            return config;
        }
    }
}
