using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class FetchPermissionsOutputModel
    {
        private FetchPermissionsOutputModel(IReadOnlyCollection<Permission> permissions)
        {
            Permissions = permissions;
        }
        public IReadOnlyCollection<Permission> Permissions { get; set; }
        public static FetchPermissionsOutputModel FromPermissions(IReadOnlyCollection<Permission> permissions)
        {
            _ = permissions ?? throw new ArgumentNullException(nameof(permissions));
            return new FetchPermissionsOutputModel(permissions);
        }
    }
}
