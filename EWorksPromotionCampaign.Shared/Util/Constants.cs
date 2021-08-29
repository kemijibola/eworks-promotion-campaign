using EWorksPromotionCampaign.Shared.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Shared.Util
{
    public class Constants
    {
        private Constants() {}
        public const string PhoneNumberRegexFormat = "^234\\d{10}";
        public const string SuperAdminRole = "super admin";
        public List<ConfigurationModel> Configurations { get; set; }

        public void GetConfigurations()
        {
            Configurations = ConfigurationsKvp();
        }

        private static List<ConfigurationModel> ConfigurationsKvp() => new()
        {
            new ConfigurationModel { Key = ConfigurationType.BlacklistedSubscriberMessage.ToString(), Value = "Blacklisted Subscriber Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.CashWiningMessage.ToString(), Value = "Cash Wining Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.AirtimeWinningMessage.ToString(), Value = "Airtime Wining Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.DailySubscriptionlimit.ToString(), Value = "Daily Subscription Limit", Type = "int" },
            new ConfigurationModel { Key = ConfigurationType.DefaultSMSMessage.ToString(), Value = "Default SMS Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.EmailPortalName.ToString(), Value = "Email Portal Name", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.EmailSenderName.ToString(), Value = "Email Sender Name", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.FailedRequestMessage.ToString(), Value = "Failed Request Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.IncompleteParametersMessage.ToString(), Value = "Incomplete Parameters Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.InvalidRewardCodeMessage.ToString(), Value = "Invalid RewardCode Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.MaximumDailyAirtimeLimit.ToString(), Value = "Maximum Daily Airtime Limit", Type = "int" },
            new ConfigurationModel { Key = ConfigurationType.MaximumDailyAirtimeLimitReachedMessage.ToString(), Value = "Maximum Daily Airtime Limit Reached Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.MaximumDailyCashLimit.ToString(), Value = "Maximum Daily Cash Limit", Type = "int" },
            new ConfigurationModel { Key = ConfigurationType.MaximumDailyCashLimitReachedMessage.ToString(), Value = "Maximum Daily Cash Limit Reached Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.MaximumDailyLimitReachedMessage.ToString(), Value = "Maximum Daily Limit Reached Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.NoActiveCampaignMessage.ToString(), Value = "No Active Campaign Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.NonExistentDailyLimitsMessage.ToString(), Value = "Non Existent Daily Limits Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.RaffleDrawSubscriptionMessage.ToString(), Value = "Raffle Draw Subscription Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.RetryWaitSeconds.ToString(), Value = "Retry Wait Seconds", Type = "int" },
            new ConfigurationModel { Key = ConfigurationType.UsedRewardCodeMessage.ToString(), Value = "Used Reward Code Message", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.WeeklyDrawDay.ToString(), Value = "Weekly Draw Day", Type = "string" },
            new ConfigurationModel { Key = ConfigurationType.CashWinningRetryLimit.ToString(), Value = "Cash Winning Retry Limit", Type = "int" },
            new ConfigurationModel { Key = ConfigurationType.AirtimeWinningRetryLimit.ToString(), Value = "Airtime Winning Retry Limit", Type = "int" },
        };

        public static Type ConfigurationValueType(string dataType)
        {
            var types = new Dictionary<string, Type>
            {
                { "string", typeof(string) },
                { "int", typeof(int) }
            };
            return types[dataType];
        }
        public static Constants Initialize()
        {
            return new Constants();
        }
    }
}
