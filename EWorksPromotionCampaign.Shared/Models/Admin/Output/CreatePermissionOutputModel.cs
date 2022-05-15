using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class CreatePermissionOutputModel
    {
        private CreatePermissionOutputModel(Permission permission)
        {
            Id = permission.Id;
            PermissionName = permission.PermissionName;
            PermissionDescription = permission.PermissionDescription;
            Status = false;
        }
        public long Id { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public bool Status { get; set; }

        public static CreatePermissionOutputModel FromPermission(Permission permission)
        {
            _ = permission ?? throw new ArgumentNullException(nameof(permission));
            return new CreatePermissionOutputModel(permission);
        }
    }
}
