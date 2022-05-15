using EWorksPromotionCampaign.Repository;
using EWorksPromotionCampaign.Tests.Fakes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where
    TStartup : class
    {
        public FakeUserRepository FakeUserRepository { get; }
        public FakePasswordRepository FakePasswordRepository { get; }
        public FakeTwoFactorRepository FakeTwoFactorRepository { get; }
        public CustomWebApplicationFactory()
        {
            Environment.SetEnvironmentVariable("env", "Test");
            FakeUserRepository = FakeUserRepository.WithDefaultUsers();
            FakePasswordRepository = FakePasswordRepository.WithDefault();
            FakeTwoFactorRepository = FakeTwoFactorRepository.WithDefaultTokenRequests();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IUserRepository>(FakeUserRepository);
                services.AddSingleton<IPasswordRepository>(FakePasswordRepository);
                services.AddSingleton<ITwoFactorRepository>(FakeTwoFactorRepository);
                services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var config = new OpenIdConnectConfiguration()
                    {
                        Issuer = FakeJwtToken.Issuer
                    };

                    config.SigningKeys.Add(FakeJwtToken.SecurityKey);
                    options.Configuration = config;
                });
            });
        }
    }
}
