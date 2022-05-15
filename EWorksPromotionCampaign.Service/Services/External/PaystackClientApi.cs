using EWorksPromotionCampaign.Shared.Models.Payment;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Services.External
{
    public interface IPaystackClientApi
    {
        Task<PaystackPaymentVerificationResponse> VerifyPayment(string reference, CancellationToken cancellationToken = default);
    }
    public class PaystackClientApi : IPaystackClientApi
    {
        private readonly ExternalServicesConfig _externalServiceConfig;
        private readonly ILogger<PaystackClientApi> _logger;
        private readonly HttpClient _httpClient;
        public PaystackClientApi(ILogger<PaystackClientApi> logger, HttpClient httpClient, IOptionsMonitor<ExternalServicesConfig> options)
        {
            _logger = logger;
            _externalServiceConfig = options.Get(ExternalServicesConfig.PaystackServiceApi);
            httpClient.BaseAddress = new Uri(_externalServiceConfig.Url);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.Timeout = new TimeSpan(0, 0, 1, 0, 0);

            _httpClient = httpClient;
        }
        public async Task<PaystackPaymentVerificationResponse> VerifyPayment(string reference, CancellationToken cancellationToken = default)
        {
            try
            {
                var url = $"{_externalServiceConfig.Url}/transaction/verify/{reference}";
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                // var paystackAuth = new AuthenticationHeaderValue("Bearer ", _externalServiceConfig.ClientSecret);
                request.Headers.Add("Authorization", $"Bearer {_externalServiceConfig.ClientSecret}");
                var response = await _httpClient.SendAsync(request, cancellationToken);
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
                return JsonConvert.DeserializeObject<PaystackPaymentVerificationResponse>(responseString);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to verify payment on Paystack API");
            }
            return null;
        }

    }
}
