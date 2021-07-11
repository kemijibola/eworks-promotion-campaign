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
        public enum CampaignStatus
        {
            inactive,
            ongoing,
            expired
        }
        public enum StartMode
        {
            Manual,
            Automatic
        }
        public enum CampaignType
        {
            Raffle,
            Instant
        }
        public enum WinningType
        {
            Airtime,
            Cash
        }
        public enum ScheduleType
        {
            Hourly,
            Daily,
            Weekly,
            Monthly
        }
        public enum WinningStatus
        {
            Inactive = 0,
            Active
        }
        public enum ScheduleWinningRule
        {
            FirstComeFirstWin = 0,
            AnyRandomUserWins,
            EveryNthSubscriber
        }
    }
}
