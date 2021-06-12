using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Util
{
    public static class Permission
    {
        #region AdminPermissions
        public const string CanCreateAdminRole = "CanCreateAdminRole";
        public const string CanViewAdminRole = "CanViewAdminRole";
        public const string CanViewAdminRoles = "CanViewAdminRoles";
        public const string CanCreateAdminUser = "CanCreateAdminUser";
        public const string CanUpdateAdminRoleStatus = "CanUpdateAdminRoleStatus";
        public const string CanUpdateAdminUserStatus = "CanUpdateAdminUserStatus";
        public const string CanUpdateAdminPermissionStatus = "CanUpdateAdminPermissionStatus";
        public const string CanAssignPermissionsToRole = "CanAssignPermissionsToRole";
        public const string CanViewUsers = "CanViewUsers";
        public const string CanViewUser = "CanViewUser";
        public const string CanViewPermissions = "CanViewPermissions";
        public const string CanViewPermission = "CanViewPermission";
        public const string CanViewAdminUser = "CanViewAdminUser";
        public const string CanViewAdminUsers = "CanViewAdminUsers";
        public const string CanCreateCampaign = "CanCreateCampaign";
        public const string CanConfigureCampaign = "CanConfigureCampaign";
        public const string CanCreateSystemConfiguration = "CanCreateSystemConfiguration";
        public const string CanEditCampaign = "CanEditCampaign";
        public const string CanStartCampaign = "CanStartCampaign";
        public const string CanPauseCampaign = "CanPauseCampaign";
        public const string CanCreateCampaignReward = "CanCreateCampaignReward";
        public const string CanCreateRaffleReward = "CanCreateRaffleReward";
        public const string CanDeleteCampaignReward = "CanDeleteCampaignReward";
        public const string CanDeleteRaffleReward = "CanDeleteRaffleReward";
        public const string CanDeActivateUser = "CanDeActivateUser";
        public const string CanBlacklistUser = "CanBlacklistUser";
        public const string CanBlacklistUsers = "CanBlacklistUsers";
        public const string CanEditAdminUser = "CanEditAdminUser";
        #endregion
    }
}
