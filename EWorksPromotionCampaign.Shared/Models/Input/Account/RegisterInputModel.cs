using EWorksPromotionCampaign.Shared.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Input
{
    public class RegisterInputModel
    {
        public long Id { get; set; }
        [Required] public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Phone { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Compare("Password", ErrorMessage = "ConfirmPassword must be the same as Password")]public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }


        public User ToUser()
        {
            var user = new User(Id)
            {
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                Phone = Phone,
                Email = Email,
                Address = Address,
                DateOfBirth = DateOfBirth
            };
            return user;
        }
    }
}
