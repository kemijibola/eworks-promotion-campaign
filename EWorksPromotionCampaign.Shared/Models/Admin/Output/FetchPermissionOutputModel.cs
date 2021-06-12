using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class FetchPermissionOutputModel
    {
        private FetchPermissionOutputModel(Permission permission)
        {
            Id = permission.Id;
            PermissionName = permission.PermissionName;
            PermissionDescription = permission.PermissionDescription;
            Status = permission.Status;
            StatusUpdatedBy = permission.StatusUpdatedBy;
            StatusComment = permission.StatusComment;
            CreatedAt = permission.CreatedAt;
            UpdatedAt = permission.UpdatedAt;
            StatusUpdatedAt = permission.StatusUpdatedAt;
        }
        public long Id { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public bool Status { get; set; }
        public long StatusUpdatedBy { get; set; }
        public string StatusComment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StatusUpdatedAt { get; set; }
        public static FetchPermissionOutputModel FromPermission(Permission permission)
        {
            _ = permission ?? throw new ArgumentNullException(nameof(permission));
            return new FetchPermissionOutputModel(permission);
        }
    }
}
