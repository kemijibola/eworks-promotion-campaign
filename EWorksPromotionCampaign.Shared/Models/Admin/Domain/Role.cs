using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Domain
{
    public class Role
    {
        public Role(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string StatusComment { get; set; }
        public bool Status { get; set; }
        public long StatusUpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StatusUpdatedAt { get; set; }
        public IEnumerable<Permission> Permissions { get; set; }
        public Role() {}

        public void WithPermissions(IEnumerable<Permission> permissions)
        {
            Permissions = permissions;
        }
    }
}
