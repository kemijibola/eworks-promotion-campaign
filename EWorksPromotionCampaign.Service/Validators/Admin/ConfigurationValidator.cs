using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Validators.Admin
{
    public interface IConfigurationValidator
    {
        ValidationResult ValidateNewConfig(CreateConfigurationInputModel model);
        ValidationResult ValidateUpdateConfig(UpdateConfigurationInputModel model);
        ValidationResult ValidateDeleteConfig(long id);
    }
    public class ConfigurationValidator : IConfigurationValidator
    {
        public ValidationResult ValidateDeleteConfig(long id)
        {
            var result = new ValidationResult();
            if (id < 1)
                result.Errors.Add(nameof(id), "Id is required.");
            return result;
        }

        public ValidationResult ValidateNewConfig(CreateConfigurationInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Configuration is required");
            var result = new ValidationResult();
            if (string.IsNullOrEmpty(model.ConfigurationKey))
                result.Errors.Add(nameof(model.ConfigurationKey), "ConfigurationKey is required.");
            if (!Enum.TryParse(model.ConfigurationKey, true, out ConfigurationType configurationType))
                result.Errors.Add(nameof(model.ConfigurationKey), "ConfigurationKey is invalid.");
            var constants = Constants.Initialize();
            constants.GetConfigurations();
            var configurationDetails = constants.Configurations.FirstOrDefault(x => x.Key.Equals(configurationType.ToString(), StringComparison.InvariantCultureIgnoreCase));
            try
            {
                var valueType = Constants.ConfigurationValueType(configurationDetails.Type);
                Convert.ChangeType(model.ConfigurationValue, valueType);
            }
            catch (Exception)
            {
                result.Errors.Add(nameof(model.ConfigurationValue), $"ConfigurationValue type is {configurationDetails.Type}.");
            }
            if (string.IsNullOrEmpty(model.ConfigurationValue))
                result.Errors.Add(nameof(model.ConfigurationValue), "ConfigurationValue is required.");
            return result;
        }

        public ValidationResult ValidateUpdateConfig(UpdateConfigurationInputModel model)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model), "Configuration is required");
            var result = new ValidationResult();
            if (model.Id < 1)
                result.Errors.Add(nameof(model.Id), "Id is required.");
            if (string.IsNullOrEmpty(model.ConfigurationKey))
                result.Errors.Add(nameof(model.ConfigurationKey), "ConfigurationKey is required.");
            if (!Enum.TryParse(model.ConfigurationKey, true, out ConfigurationType configurationType))
                result.Errors.Add(nameof(model.ConfigurationKey), "ConfigurationKey is invalid.");
            var constants = Constants.Initialize();
            constants.GetConfigurations();
            var configurationDetails = constants.Configurations.FirstOrDefault(x => x.Key.Equals(configurationType.ToString(), StringComparison.InvariantCultureIgnoreCase));
            try
            {
                var valueType = Constants.ConfigurationValueType(configurationDetails.Type);
                Convert.ChangeType(model.ConfigurationValue, valueType);
            }
            catch (Exception)
            {
                result.Errors.Add(nameof(model.ConfigurationValue), $"ConfigurationValue type is {configurationDetails.Type}.");
            }
            if (string.IsNullOrEmpty(model.ConfigurationValue))
                result.Errors.Add(nameof(model.ConfigurationValue), "ConfigurationValue is required.");
            return result;
        }
    }
}
