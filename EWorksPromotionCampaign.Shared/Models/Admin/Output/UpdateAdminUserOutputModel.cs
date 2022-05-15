using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminUser = EWorksPromotionCampaign.Shared.Models.Admin.Domain.User;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class UpdateAdminUserOutputModel
    {
        private UpdateAdminUserOutputModel(AdminUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Phone = user.Phone;
            Email = user.Email;
            RoleId = user.RoleId;
        }
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public static UpdateAdminUserOutputModel FromUser(AdminUser user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            return new UpdateAdminUserOutputModel(user);
        }
    }
}
