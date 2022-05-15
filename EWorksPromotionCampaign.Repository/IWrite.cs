using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Repository
{
    public interface IWrite<T>
    {
        Task<long> Create(T newItem);
        Task Update<TItem>(TItem id, T updateItem);
        Task Delete<TItem>(TItem id);
    }
}
