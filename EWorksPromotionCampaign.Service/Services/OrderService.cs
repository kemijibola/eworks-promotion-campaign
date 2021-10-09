using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Service.Services.External;
using EWorksPromotionCampaign.Service.Util;
using EWorksPromotionCampaign.Service.Validators;
using EWorksPromotionCampaign.Shared.Exceptions;
using EWorksPromotionCampaign.Shared.Models.Input.Order;
using EWorksPromotionCampaign.Shared.Models.Output.Order;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Services
{
    public interface IOrderService
    {
        Task<Result<CreateOrderOutputModel>> CreateOrder(CreateOrderInputModel model);
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<CreateOrderOutputModel>> CreateOrder(CreateOrderInputModel model)
        {
            var order = model.ToOrder();
            var result = await _orderRepository.Create(order);
            var results = result.Split(':');
            var (statusCode, responseCode) = Helper.MapDbResponseCodeToStatusCode(results[0]);
            if (!responseCode.Equals(ResponseCodes.Success))
                throw new ServiceException(responseCode, results[1].Trim(), statusCode);
            order.Id = Convert.ToInt32(results[1]);
            order.Reference = results[2];
            return new Result<CreateOrderOutputModel>(new ValidationResult(), CreateOrderOutputModel.FromOrder(order));
        }
    }
}
