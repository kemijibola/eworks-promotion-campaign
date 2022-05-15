using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminUser = EWorksPromotionCampaign.Shared.Models.Admin.Domain.User;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class AdminLoginOutputModel
    {
        private AdminLoginOutputModel(AdminUser user, Role userRole, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
            RoleId = user.RoleId;
            Status = user.Status;
            IsAdmin = user.IsAdmin;
            IsDisabled = user.IsDisabled;
            Token = token;
            Role = userRole;
        }
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDisabled { get; set; }
        public string Token { get; set; }
        public Role Role { get; set; }
        public static AdminLoginOutputModel FromUser(AdminUser user, Role userRole, string token)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            return new AdminLoginOutputModel(user, userRole, token);
        }
    }
}
