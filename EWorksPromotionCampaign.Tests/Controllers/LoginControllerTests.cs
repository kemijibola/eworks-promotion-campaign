using EWorksPromotionCampaign.Service;
using EWorksPromotionCampaign.Shared.Models.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace EWorksPromotionCampaign.Tests.Controllers
{
    public class LoginControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public LoginControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/v1/");

            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task Post_LoginWithValidCredentials_ReturnsOkResult()
        {
            var content = GetValidLoginJsonContent();
            var response = await _client.PostAsync("login", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_LoginWithNonExistingUser_ReturnsNotFound()
        {
            var content = GetInvalidUserLoginJsonContent();
            var response = await _client.PostAsync("login", content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Post_LoginWithNoEmailOrPhone_ReturnsBadRequest()
        {
            var content = GetNoEmailOrPhoneLoginJsonContent();
            var response = await _client.PostAsync("login", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        private static JsonContent GetValidLoginJsonContent()
        {
            return JsonContent.Create(GetValidLoginInputModel());
        }
        private static LoginInputModel GetValidLoginInputModel()
        {
            return new LoginInputModel
            {
                Phone = "",
                Password = "Password",
                Email = "kemijibbola@gmail.com"
            };
        }
        private static JsonContent GetInvalidUserLoginJsonContent()
        {
            return JsonContent.Create(GetInvalidUserLoginInputModel());
        }
        private static LoginInputModel GetInvalidUserLoginInputModel()
        {
            return new LoginInputModel
            {
                Phone = "",
                Password = "Password",
                Email = "kemijibbolasss@gmail.com"
            };
        }
        private static JsonContent GetNoEmailOrPhoneLoginJsonContent()
        {
            return JsonContent.Create(GetNoEmailOrPhoneLoginInputModel());
        }
        private static LoginInputModel GetNoEmailOrPhoneLoginInputModel()
        {
            return new LoginInputModel
            {
                Phone = "",
                Password = "Password",
                Email = ""
            };
        }
    }
}
