using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Util
{
    public static class Enums
    {
        public enum IdentityClaimType
        {
            name,
            role
        }
        public enum TokenType
        {
            otp,
            jwt,
            hashed
        }
        public enum DigitType
        {
            alphanumeric,
            numeric
        }
        public enum ConfigurationType
        {
            DailySubscriptionlimit,
            RetryWaitSeconds,
            NonExistentDailyLimitsMessage,
            MaximumDailyLimitReachedMessage,
            NoActiveCampaignMessage,
            BlacklistedSubscriberMessage,
            InvalidRewardCodeMessage,
            UsedRewardCodeMessage,
            RaffleDrawSubscriptionMessage,
            IncompleteParametersMessage,
            FailedRequestMessage,
            WeeklyDrawDay,
            EmailSenderName,
            EmailPortalName,
            DefaultSMSMessage,
            MaximumDailyAirtimeLimit,
            MaximumDailyCashLimit,
            MaximumDailyAirtimeLimitReachedMessage,
            MaximumDailyCashLimitReachedMessage,
            CashWiningMessage
        }
    }
}
