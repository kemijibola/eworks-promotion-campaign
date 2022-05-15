using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class UpdateRoleStatusOutputModel
    {
        private UpdateRoleStatusOutputModel(Role role)
        {
            Id = role.Id;
            RoleName = role.RoleName;
            RoleDescription = role.RoleDescription;
            Status = role.Status;
            StatusUpdatedBy = role.StatusUpdatedBy;
            CreatedAt = role.CreatedAt;
            UpdatedAt = role.UpdatedAt;
            StatusUpdatedAt = role.StatusUpdatedAt;
        }
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool Status { get; set; }
        public long StatusUpdatedBy { get; set; }
        public string StatusComment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StatusUpdatedAt { get; set; }
        public static UpdateRoleStatusOutputModel FromPermission(Role role)
        {
            _ = role ?? throw new ArgumentNullException(nameof(role));
            return new UpdateRoleStatusOutputModel(role);
        }
    }
}
