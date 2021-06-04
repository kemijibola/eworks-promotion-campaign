using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Repository
{
    public interface IRead<T>
    {
        Task<IReadOnlyCollection<T>> Fetch();
        Task<T> FindById<TItem>(TItem id);
    }
}
