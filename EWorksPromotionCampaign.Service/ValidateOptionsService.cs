using EWorksPromotionCampaign.Service.Services.External;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service
{
    public class ValidateOptionsService : IHostedService
    {
        private readonly ILogger<ValidateOptionsService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IOptionsMonitor<ExternalServicesConfig> _externalServicesConfig;
        public ValidateOptionsService(
            ILogger<ValidateOptionsService> logger,
            IHostApplicationLifetime appLifetime,
            IOptionsMonitor<ExternalServicesConfig> externalServicesConfig)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _externalServicesConfig = externalServicesConfig;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _ = _externalServicesConfig.Get(ExternalServicesConfig.QuickTellerServiceApi);
            }
            catch (OptionsValidationException ex)
            {
                _logger.LogError("One or more options validation checks failed.");

                foreach (var failure in ex.Failures)
                {
                    _logger.LogError(failure);
                }

                _appLifetime.StopApplication();
            }

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
