using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class UpdateRoleInputModel
    {
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public Role ToRole()
        {
            var role = new Role(Id)
            {
                RoleName = RoleName,
                RoleDescription = RoleDescription
            };
            return role;
        }
    }
}
