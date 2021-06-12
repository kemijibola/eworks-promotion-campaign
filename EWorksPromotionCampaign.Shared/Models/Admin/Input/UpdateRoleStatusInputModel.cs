using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class UpdateRoleStatusInputModel
    {
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; }

        public Role ToRole()
        {
            var role = new Role(RoleId)
            {
                Status = Status,
                StatusUpdatedBy = UserId,
                StatusComment = Comment
            };
            return role;
        }
    }
}
