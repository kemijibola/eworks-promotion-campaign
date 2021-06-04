using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Service.Util;
using EWorksPromotionCampaign.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Tests.Fakes
{
    public class FakeTwoFactorRepository : ITwoFactorRepository
    {
        private IReadOnlyCollection<ITwoFactorAuthentication> _customDefaultTokenRequests;
        public List<ITwoFactorAuthentication> TokenRequests { get; set; }

        public FakeTwoFactorRepository(IReadOnlyCollection<ITwoFactorAuthentication> customDefaultTokenRequests = null)
        {
            _customDefaultTokenRequests = customDefaultTokenRequests;
        }
        public Task<long> Create(ITwoFactorAuthentication newItem)
        {
            TokenRequests.Add(newItem);
            return Task.FromResult(10000000000000);
        }

        public Task Delete<TItem>(TItem id)
        {
            return Task.FromResult(TokenRequests.Where(p => p.RequestId != id.ToString()));
        }

        public Task<IReadOnlyCollection<ITwoFactorAuthentication>> Fetch()
        {
            return Task.FromResult(TokenRequests as IReadOnlyCollection<ITwoFactorAuthentication>);
        }

        public Task<ITwoFactorAuthentication> FindById<TItem>(TItem id)
        {
            return Task.FromResult(TokenRequests.SingleOrDefault(p => p.RequestId == id.ToString()));
        }

        public Task<ITwoFactorAuthentication> GetTokenReqestByToken(string token)
        {
            return Task.FromResult(TokenRequests.SingleOrDefault(p => p.Token == token));
        }

        public Task Update<TItem>(TItem id, ITwoFactorAuthentication updateItem)
        {
            throw new NotImplementedException();
        }
        public void ResetDefaultTokenRequests(bool useCustomIfAvailable = true)
        {
            TokenRequests = _customDefaultTokenRequests is object && useCustomIfAvailable
                ? _customDefaultTokenRequests.ToList()
                : GetDefaultTokenRequests();
        }
        public static FakeTwoFactorRepository WithDefaultTokenRequests()
        {
            var passwordRepository = new FakeTwoFactorRepository();
            passwordRepository.ResetDefaultTokenRequests();
            return passwordRepository;
        }
        private List<ITwoFactorAuthentication> GetDefaultTokenRequests() => new()
        {
            new TwoFactorAuthentication
            {
                Requester = "kemijibbola@gmail.com",
                Salt = "lPs4AY2QjzU7+aslEpOUVQ==",
                Token = "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJleHAiOjE2MTkzODg3ODF9.",
                CreatedAt = DateTime.UtcNow.ToUniversalTime(),
                TypeOfToken = Enums.TokenType.jwt.ToString(),
                RequestId = "7ea69230-a965-406f-b14b-7a0033ab7897"
            },
            new TwoFactorAuthentication
            {
                Requester = "1",
                Salt = "",
                Token = "5C96B5F33E98C547F79A01F048E1635E3756AF11",
                CreatedAt = DateTime.UtcNow.ToUniversalTime(),
                TypeOfToken = Enums.TokenType.hashed.ToString(),
                RequestId = "e528b7e4-657c-4b42-94e3-b18b0efef641"
            }
        };
    }
}
