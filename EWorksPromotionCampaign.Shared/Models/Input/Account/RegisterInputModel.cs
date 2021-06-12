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
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        [EmailAddress] public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
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
