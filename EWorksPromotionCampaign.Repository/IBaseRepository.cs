using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Repository
{
    public interface IBaseRepository<T> : IRead<T>, IWrite<T> where T : class { }
}
