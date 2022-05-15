using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Services.External
{
    public class ExternalServicesConfigurationValidation : IValidateOptions<ExternalServicesConfig>
    {
        public ValidateOptionsResult Validate(string name, ExternalServicesConfig options)
        {
            switch (name)
            {
                case ExternalServicesConfig.QuickTellerServiceApi:
                    var result = ValidateQuickTellerApiConfig(options);
                    if (result.Failed)
                        return result;
                    break;
                case ExternalServicesConfig.PaystackServiceApi:
                    var paystackResult = ValidatePaystackApiConfig(options);
                    if (paystackResult.Failed)
                        return paystackResult;
                    break;
                default:
                    return ValidateOptionsResult.Skip;
            }

            return ValidateOptionsResult.Success;
        }
        public static ValidateOptionsResult ValidateQuickTellerApiConfig(ExternalServicesConfig options)
        {
            if (string.IsNullOrEmpty(options.Url))
            {
                return ValidateOptionsResult.Fail("A URL for QuickTeller API is required.");
            }
            if (string.IsNullOrEmpty(options.ClientId))
            {
                return ValidateOptionsResult.Fail("A ClientId for QuickTeller API is required.");
            }
            if (string.IsNullOrEmpty(options.ClientSecret))
            {
                return ValidateOptionsResult.Fail("A ClientSecret for QuickTeller API is required.");
            }
            return ValidateOptionsResult.Success;
        }
        public static ValidateOptionsResult ValidatePaystackApiConfig(ExternalServicesConfig options)
        {
            if (string.IsNullOrEmpty(options.Url))
            {
                return ValidateOptionsResult.Fail("A URL for Paystack API is required.");
            }
            if (string.IsNullOrEmpty(options.ClientId))
            {
                return ValidateOptionsResult.Fail("A ClientId for Paystack API is required.");
            }
            if (string.IsNullOrEmpty(options.ClientSecret))
            {
                return ValidateOptionsResult.Fail("A ClientSecret for Paystack API is required.");
            }
            return ValidateOptionsResult.Success;
        }
    }
}
