using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class FetchRolesOutputModel
    {
        private FetchRolesOutputModel(IReadOnlyCollection<Role> roles)
        {
            Roles = roles;
        }
        public IReadOnlyCollection<Role> Roles { get; set; }
        public static FetchRolesOutputModel FromRoles(IReadOnlyCollection<Role> roles)
        {
            _ = roles ?? throw new ArgumentNullException(nameof(roles));
            return new FetchRolesOutputModel(roles);
        }
    }
}
