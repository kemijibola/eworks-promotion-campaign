using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Service.Validators.Admin;
using EWorksPromotionCampaign.Shared.Models.Admin;
using EWorksPromotionCampaign.Shared.Models.Admin.Input;
using EWorksPromotionCampaign.Shared.Models.Admin.Output;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Services.Admin
{
    public interface IConfigurationService
    {
        Task<AddResult<CreateConfigurationOutputModel>> CreateConfiguration(CreateConfigurationInputModel model);
        Task<Result<UpdateConfigurationOutputModel>> UpdateConfiguration(UpdateConfigurationInputModel model);
        Task<Result<string>> DeleteConfiguration(long id);
        Result<IReadOnlyCollection<ConfigurationModel>> GetConfigurationKeys();
        Task<Result<FetchConfigurationsOutputModel>> GetConfigurations();
    }
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationValidator _configurationValidator;
        private readonly IConfigurationRepository _configurationRepository;
        public ConfigurationService(IConfigurationValidator configurationValidator, IConfigurationRepository configurationRepository)
        {
            _configurationValidator = configurationValidator;
            _configurationRepository = configurationRepository;
        }

        public async Task<AddResult<CreateConfigurationOutputModel>> CreateConfiguration(CreateConfigurationInputModel model)
        {
            var validationResult = _configurationValidator.ValidateNewConfig(model);
            var existingConfiguration = await _configurationRepository.FindByType(model.ConfigurationKey);
            if (validationResult.IsValid && existingConfiguration is null)
            {
                var config = model.ToConfiguration();
                config.Id = await _configurationRepository.Create(config);
                return new AddResult<CreateConfigurationOutputModel>(validationResult, false, CreateConfigurationOutputModel.FromConfiguration(config));
            }
            return new AddResult<CreateConfigurationOutputModel>(validationResult, validationResult.IsValid, null);
        }

        public async Task<Result<string>> DeleteConfiguration(long id)
        {
            var validationResult = _configurationValidator.ValidateDeleteConfig(id);
            if (validationResult.IsValid)
            {
                await _configurationRepository.Delete(id);
                return new Result<string>(validationResult, "Configuration deleted successfully");
            }
            return new Result<string>(validationResult, null);
        }

        public Result<IReadOnlyCollection<ConfigurationModel>> GetConfigurationKeys()
        {
            var constants = Constants.Initialize();
            constants.GetConfigurations();
            return new Result<IReadOnlyCollection<ConfigurationModel>>(new ValidationResult(), constants.Configurations);
        }

        public async Task<Result<FetchConfigurationsOutputModel>> GetConfigurations()
        {
            var configurations = await _configurationRepository.Fetch();
            return new Result<FetchConfigurationsOutputModel>(new ValidationResult(), FetchConfigurationsOutputModel.FromConfigurations(configurations));
        }

        public async Task<Result<UpdateConfigurationOutputModel>> UpdateConfiguration(UpdateConfigurationInputModel model)
        {
            var validationResult = _configurationValidator.ValidateUpdateConfig(model);
            if (validationResult.IsValid)
            {
                var configuration = model.ToConfiguration();
                await _configurationRepository.Update(model.Id, configuration);
                return new Result<UpdateConfigurationOutputModel>(validationResult, UpdateConfigurationOutputModel.FromConfiguration(configuration));
            }
            return new Result<UpdateConfigurationOutputModel>(validationResult, null);
        }
    }
}
