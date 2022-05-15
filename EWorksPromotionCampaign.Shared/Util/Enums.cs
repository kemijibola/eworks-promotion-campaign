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
            CashWiningMessage,
            AirtimeWinningMessage,
            CashWinningRetryLimit,
            AirtimeWinningRetryLimit,
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
        public enum RewardStatus
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
        public enum SubscriberStatus
        {
            Inactive = 0,
            Subscribed = 1,
            Drawn = 2,
            Won = 3
        }

        public enum RaffleRewardTypeStatus
        {
            NotDrawn, 
            Drawn
        }
    }
}
