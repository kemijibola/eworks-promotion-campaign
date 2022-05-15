using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Shared.Models.Domain
{
    public class User
    {
        public User(long id)
        {
            Id = id;
        }
        public User() { }
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsDeactivated { get; set; }
        public bool IsDisabled { get; set; }
        public bool LockedOutEnabled { get; set; }
        public bool Status { get; set; }
        public string StatusComment { get; set; }
        public long StatusUpdatedBy { get; set; }
        public bool LockedOut { get; set; }
        public int AccessFailedCount { get; set; }
        public string DisabledComment { get; set; }
        public DateTime DisabledAt { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LockedOutAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StatusUpdatedAt { get; set; }

        public void UpdateAccessFailedCount()
        {
            AccessFailedCount++;
        }
        public AuthModel GenerateToken(ITokenHandler tokenHandler)
        {
            _ = Email ?? throw new ArgumentNullException(nameof(Email));

            var tokenRequest = new GenerateTokenRequest()
            {
                ClaimTypes = new Dictionary<IdentityClaimType, string>()
                {
                    { IdentityClaimType.name, Email },
                    { IdentityClaimType.role, "User" }
                }
            };

            //var refreshTokenRequest = new GenerateTokenRequest()
            //{
            //    ClaimTypes = new Dictionary<IdentityClaimType, string>()
            //    {
            //        { IdentityClaimType.name, Email },
            //        { IdentityClaimType.role, "Refresh" }
            //    }
            //};

            return new AuthModel
            {
                Token = tokenHandler.GenerateToken(tokenRequest)
            };
        }
    }
}
