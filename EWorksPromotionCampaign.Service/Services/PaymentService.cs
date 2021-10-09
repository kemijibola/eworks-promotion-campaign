using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Services.External;
using EWorksPromotionCampaign.Service.Util;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models.Domain;
using EWorksPromotionCampaign.Shared.Models.Input.Order;
using EWorksPromotionCampaign.Shared.Models.Input.Payment;
using EWorksPromotionCampaign.Shared.Models.Output.Payment;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Services
{
    public interface IPaymentService
    {
        Task<Result<ConfirmPaymentOutputModel>> ConfirmPayment(ConfirmPaymentInputModel model);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IPaystackClientApi _paystackClientApi;
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaystackClientApi paystackClientApi, IPaymentRepository paymentRepository)
        {
            _paystackClientApi = paystackClientApi;
            _paymentRepository = paymentRepository;
        }

        public async Task<Result<ConfirmPaymentOutputModel>> ConfirmPayment(ConfirmPaymentInputModel model)
        {
            var verificationResponse = await _paystackClientApi.VerifyPayment(model.Reference);
            if (verificationResponse is null)
                throw new ServiceException(ResponseCodes.ProcessingError, "Unable to complete at this time. Please try again later.", 422);
            if (!verificationResponse.Status)
                throw new ServiceException(ResponseCodes.ProcessingError, verificationResponse.Message, 412);
            if (!verificationResponse.Data.Customer.Email.Equals(model.CustomerEmail))
                throw new ServiceException(ResponseCodes.InvalidRequest, "Invalid payment - Customer Email" , 412);
            var payment = model.ToPayment(verificationResponse);
            var result = await _paymentRepository.Create(payment, model.CustomerEmail);
            var results = result.Split(':');
            var (statusCode, responseCode) = Helper.MapDbResponseCodeToStatusCode(results[0]);
            if (!responseCode.Equals(ResponseCodes.Success))
                throw new ServiceException(responseCode, results[1].Trim(), statusCode);
            payment.Id = Convert.ToInt32(results[1]);
            return new Result<ConfirmPaymentOutputModel>(new ValidationResult(), ConfirmPaymentOutputModel.FromPayment(payment));
        }
    }
}
