using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class AdminLoginInputModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required] public string Password { get; set; }
    }
}
