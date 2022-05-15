using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Tests.Models
{
    public class TestRegisterInputModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TestRegisterInputModel CloneWith(Action<TestRegisterInputModel> changes)
        {
            var clone = (TestRegisterInputModel)MemberwiseClone();

            changes(clone);

            return clone;
        }
    }
}
