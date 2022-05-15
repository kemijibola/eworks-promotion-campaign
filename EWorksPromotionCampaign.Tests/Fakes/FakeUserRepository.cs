using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Shared.Models.Admin;
using EWorksPromotionCampaign.Shared.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminUser = EWorksPromotionCampaign.Shared.Models.Admin.Domain.User;

namespace EWorksPromotionCampaign.Tests.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        private IReadOnlyCollection<User> _customDefaultUsers;
        public List<User> Users { get; set; }

        public FakeUserRepository(IReadOnlyCollection<User> users = null)
        {
            ReplaceCustomProducts(users);

        }
        public Task<long> Create(User newItem)
        {
            Users.Add(newItem);
            return Task.FromResult(10000000000000);
        }

        public Task Delete<TItem>(TItem id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<User>> Fetch()
        {
            return Task.FromResult(Users as IReadOnlyCollection<User>);
        }
        public Task<User> FindById<TItem>(TItem id)
        {
            var itemId = Convert.ToInt64(id);
            return Task.FromResult(Users.SingleOrDefault(p => p.Id == itemId));
        }

        public Task Update<TItem>(TItem id, User updateItem)
        {
            var itemId = Convert.ToInt64(id);
            var indexOfUser = Users.FindIndex(x => x.Id == itemId);
            if (indexOfUser != -1)
            {
                Users[indexOfUser].FirstName = updateItem.FirstName;
                Users[indexOfUser].MiddleName = updateItem.MiddleName;
                Users[indexOfUser].LastName = updateItem.LastName;
                Users[indexOfUser].Phone = updateItem.Phone;
                Users[indexOfUser].Email = updateItem.Email;
                Users[indexOfUser].Address = updateItem.Address;
                Users[indexOfUser].DateOfBirth = updateItem.DateOfBirth;
            }
            return Task.CompletedTask;
        }
        public void ReplaceCustomProducts(IReadOnlyCollection<User> users)
        {
            _customDefaultUsers = users;
        }
        public void ResetDefaultUsers(bool useCustomIfAvailable = true)
        {
            Users = _customDefaultUsers is object && useCustomIfAvailable
                ? _customDefaultUsers.ToList()
                : GetDefaultUsers();
        }
        private List<User> GetDefaultUsers() => new()
        {
            new User
            {
                Id = 1,
                FirstName = "Oluwakemi",
                LastName = "Awosile",
                MiddleName = "",
                Phone = "08080737373",
                Email = "kemijibbola@gmail.com",
                Address = "Ibadan,Oyo-State",
                DateOfBirth = DateTime.UtcNow.ToUniversalTime(),
                PasswordHash = "HjdvnXivBiByhfKT5BtpXaQsFCiPmqnjmhdsDd95gXA=",
                PasswordSalt = "lPs4AY2QjzU7+aslEpOUVQ=="
            },
            new User
            {
                Id = 2,
                FirstName = "Seyifumi",
                LastName = "Sanni",
                MiddleName = "",
                Phone = "0807837333",
                Email = "seyifunmi@gmail.com",
                Address = "Ibadan,Oyo-State",
                DateOfBirth = DateTime.UtcNow.ToUniversalTime(),
                PasswordHash = "HjdvnXivBiByhfKT5BtpXaQsFCiPmqnjmhdsDd95gXA=",
                PasswordSalt = "lPs4AY2QjzU7+aslEpOUVQ=="
            },
            new User
            {
                Id = 3,
                FirstName = "Johnn",
                LastName = "Doe",
                MiddleName = "",
                Phone = "08080747474",
                Email = "john.doe@gmail.com",
                Address = "Ibadan,Oyo-State",
                DateOfBirth = DateTime.UtcNow.ToUniversalTime(),
                PasswordHash = "HjdvnXivBiByhfKT5BtpXaQsFCiPmqnjmhdsDd95gXA=",
                PasswordSalt = "lPs4AY2QjzU7+aslEpOUVQ=="
            }
        };

        public Task<User> GetUserByEmailOrPhone(string email, string phone)
        {
            return Task.FromResult(Users.SingleOrDefault(p => p.Email == email || p.Phone == phone));
        }

        public Task<User> GetUserByIdentifier(string identifier)
        {
            return Task.FromResult(Users.SingleOrDefault(p => p.Email == identifier || p.Phone == identifier));
        }

        public Task<User> GetUserByEmail(string email)
        {
            return Task.FromResult(Users.SingleOrDefault(p => p.Email == email));
        }
        public static FakeUserRepository WithDefaultUsers()
        {
            var userRepository = new FakeUserRepository();
            userRepository.ResetDefaultUsers();
            return userRepository;
        }
        public Task<IReadOnlyCollection<User>> FindAllByCriteria<TItem>(TItem criterias)
        {
            throw new NotImplementedException();
        }

        public Task<AdminUser> GetAdminUserByIdetifier(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAdminUser(AdminUser user)
        {
            throw new NotImplementedException();
        }

        public Task<AdminUser> GetAdminUserByEmailOrPhone(string email, string phone)
        {
            throw new NotImplementedException();
        }

        public Task<AdminUser> GetAdminUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAdminUser(AdminUser user)
        {
            throw new NotImplementedException();
        }

        public Task<AdminUser> FindAdminById(long id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAdminUserStatus(AdminUser user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAdminUserDisabledStatus(AdminUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<AdminUserOverview>> GetAdminUserOverviews(int pageNumber, int pageSize, string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
