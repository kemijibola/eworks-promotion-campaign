using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class CreatePermissionInputModel
    {
        public long Id { get; set; }
        [Required] public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }

        public Permission ToPermission()
        {
            return new Permission
            {
                PermissionName = PermissionName,
                PermissionDescription = PermissionDescription
            };
        }
    }
}
