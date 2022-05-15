using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Output
{
    public class FetchUserByEmailOutputModel
    {
        private FetchUserByEmailOutputModel(Domain.User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Phone = user.Phone;
            Email = user.Email;
            Status = true;
            IsDisabled = user.IsDisabled;
            LockedOut = user.LockedOut;
        }
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public bool Status { get; set; }
        public bool LockedOut { get; set; }
        public static FetchUserByEmailOutputModel FromUser(Domain.User user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            return new FetchUserByEmailOutputModel(user);
        }
    }
}
