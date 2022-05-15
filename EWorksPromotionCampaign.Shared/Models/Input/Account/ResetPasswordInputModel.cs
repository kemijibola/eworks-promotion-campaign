using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Input.Account
{
    public class ResetPasswordInputModel
    {
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Compare("Password", ErrorMessage = "ConfirmPassword must be the same as Password")]public string ConfirmPassword { get; set; }
        [Required] public string Token { get; set; }
    }
}
