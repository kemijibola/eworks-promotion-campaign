using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Domain
{
    public class Permission
    {
        public Permission(int id)
        {
            Id = id;
        }
        public Permission() { }
        public long Id { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public string StatusComment { get; set; }
        public bool Status { get; set; }
        public long StatusUpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StatusUpdatedAt { get; set; }
    }
}
