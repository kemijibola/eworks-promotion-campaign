using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class FetchRolePermissionOutputModel
    {
        private FetchRolePermissionOutputModel(Role role)
        {
            Id = role.Id;
            RoleName = role.RoleName;
            RoleDescription = role.RoleDescription;
            Status = role.Status;
            StatusComment = role.StatusComment;
            StatusUpdatedBy = role.StatusUpdatedBy;
            CreatedAt = role.CreatedAt;
            UpdatedAt = role.UpdatedAt;
            StatusUpdatedAt = role.StatusUpdatedAt;
            Permissions = role.Permissions;
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
        public IEnumerable<Permission> Permissions { get; set; }
        public static FetchRolePermissionOutputModel FromRolePermission(Role role)
        {
            _ = role ?? throw new ArgumentNullException(nameof(role));
            return new FetchRolePermissionOutputModel(role);
        }

        public static Role ToRolePermission(FetchRolePermissionOutputModel role)
        {
            return new Role(role.Id)
            {
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
                Status = role.Status,
                StatusComment = role.StatusComment,
                StatusUpdatedBy = role.StatusUpdatedBy,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt,
                StatusUpdatedAt = role.StatusUpdatedAt,
                Permissions = role.Permissions,
            };
        }
    }
}
