using EWorksPromotionCampaign.Shared.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class CreateConfigurationOutputModel
    {
        private CreateConfigurationOutputModel(Configuration configuration)
        {
            Id = configuration.Id;
            ConfigurationKey = configuration.Key;
            ConfigurationValue = configuration.Value;
        }
        public long Id { get; set; }
        public string ConfigurationKey { get; set; }
        public string ConfigurationValue { get; set; }
        public static CreateConfigurationOutputModel FromConfiguration(Configuration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
            return new CreateConfigurationOutputModel(configuration);
        }
    }
}
