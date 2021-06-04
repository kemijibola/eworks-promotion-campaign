using EWorksPromotionCampaign.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Tests.Fakes
{
    public class FakePasswordRepository : IPasswordRepository
    {
        public static FakePasswordRepository WithDefault()
        {
            return new FakePasswordRepository();
        }
        public Task UpdateUserPasswordData(string email, string passwordHash, string passwordSalt)
        {
            return Task.CompletedTask;
        }
    }
}
