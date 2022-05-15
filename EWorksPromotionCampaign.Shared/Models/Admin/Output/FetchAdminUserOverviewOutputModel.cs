using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class FetchAdminUserOverviewOutputModel
    {
        private FetchAdminUserOverviewOutputModel(IReadOnlyCollection<AdminUserOverview> adminUsers)
        {
            AdminUsers = adminUsers;
        }
        public IReadOnlyCollection<AdminUserOverview> AdminUsers { get; set; }
        public static FetchAdminUserOverviewOutputModel FromAdminUsers(IReadOnlyCollection<AdminUserOverview> users)
        {
            _ = users ?? throw new ArgumentNullException(nameof(users));
            return new FetchAdminUserOverviewOutputModel(users);
        }
    }
}
