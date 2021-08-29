using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin
{
    public class AdminUserOverview
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
