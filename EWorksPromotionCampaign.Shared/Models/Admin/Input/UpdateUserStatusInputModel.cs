using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminUser = EWorksPromotionCampaign.Shared.Models.Admin.Domain.User;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class UpdateUserStatusInputModel
    {
        [Required] public int Id { get; set; }
        [Required] public bool Status { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; }

        public AdminUser ToUser()
        {
            var user = new AdminUser(Id)
            {
                Status = Status,
                StatusUpdatedBy = UserId,
                StatusComment = Comment
            };
            return user;
        }
    }
}
