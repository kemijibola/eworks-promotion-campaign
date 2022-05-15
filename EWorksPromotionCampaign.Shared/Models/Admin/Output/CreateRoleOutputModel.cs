using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class CreateRoleOutputModel
    {
        private CreateRoleOutputModel(Role role)
        {
            Id = role.Id;
            RoleName = role.RoleName;
            RoleDescription = role.RoleDescription;
            Status = false;
        }
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool Status { get; set; }
        public static CreateRoleOutputModel FromRole(Role role)
        {
            _ = role ?? throw new ArgumentNullException(nameof(role));
            return new CreateRoleOutputModel(role);
        }
    }
}
