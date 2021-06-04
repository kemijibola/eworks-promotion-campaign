using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Input.Account
{
    public class ForgotPasswordInputModel
    {
        [EmailAddress] public string Email { get; set; }
    }
}
