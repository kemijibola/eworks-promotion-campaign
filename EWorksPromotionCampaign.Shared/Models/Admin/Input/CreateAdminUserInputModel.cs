using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminUser = EWorksPromotionCampaign.Shared.Models.Admin.Domain.User;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class CreateAdminUserInputModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress] public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }

        public AdminUser ToUser()
        {
            var user = new AdminUser(Id)
            {
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                Email = Email,
                RoleId = RoleId
            };
            return user;
        }
    }
}
