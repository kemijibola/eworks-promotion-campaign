using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class BaseAdminUserOutputModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool IsDeactivated { get; set; }
        public long StatusUpdatedBy { get; set; }
        public bool LockedOutEnabled { get; set; }
        public bool LockedOut { get; set; }
        public bool Status { get; set; }
        public string StatusComment { get; set; }
        public bool IsFirstLogin { get; set; }
        public bool IsAdmin { get; set; }
        public int AccessFailedCount { get; set; }
        public string DisabledComment { get; set; }
        public DateTime DisabledAt { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LockedOutAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StatusUpdatedAt { get; set; }
    }
}
