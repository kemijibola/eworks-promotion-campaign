using EWorksPromotionCampaign.Service.Data;
using EWorksPromotionCampaign.Shared.Models.Input.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Services
{
    public interface IOrderService
    {
        Task<Result<string>> CreateOrder(CreateOrderInputModel model);
    }
    public class OrderService
    {
    }
}
