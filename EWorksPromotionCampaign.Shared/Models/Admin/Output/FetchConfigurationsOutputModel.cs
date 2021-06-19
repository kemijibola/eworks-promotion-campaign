using EWorksPromotionCampaign.Shared.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class FetchConfigurationsOutputModel
    {
        private FetchConfigurationsOutputModel(IReadOnlyCollection<Configuration> configurations)
        {
            Configurations = configurations;
        }
        public IReadOnlyCollection<Configuration> Configurations { get; set; }
        public static FetchConfigurationsOutputModel FromConfigurations(IReadOnlyCollection<Configuration> configurations)
        {
            _ = configurations ?? throw new ArgumentNullException(nameof(configurations));
            return new FetchConfigurationsOutputModel(configurations);
        }
    }
}
