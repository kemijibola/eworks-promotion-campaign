using EWorksPromotionCampaign.Service;
using EWorksPromotionCampaign.Shared.Models.Input;
using EWorksPromotionCampaign.Shared.Models.Input.Account;
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
    public class PasswordControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public PasswordControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/v1/");

            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task Post_ForgotPassword_ReturnsOk()
        {
            var content = GetValidForgotPasswordJsonContent();
            var response = await _client.PostAsync("password/forgot", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_ResetPasswordWithInvalidEmailToken_ReturnsNotFound()
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.";
            var content = GetValidResetPasswordJsonContent(token);
            var response = await _client.PostAsync("password/reset", content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        private static JsonContent GetValidForgotPasswordJsonContent()
        {
            return JsonContent.Create(GetValidForgotPasswordInputModel());
        }
        private static ForgotPasswordInputModel GetValidForgotPasswordInputModel()
        {
            return new ForgotPasswordInputModel
            {
                Email = "kemijibbola@gmail.com"
            };
        }
        private static JsonContent GetValidResetPasswordJsonContent(string generatedToken)
        {
            return JsonContent.Create(GetValidResetPasswordInputModel(generatedToken));
        }
        private static ResetPasswordInputModel GetValidResetPasswordInputModel(string generatedToken)
        {
            return new ResetPasswordInputModel
            {
                Email = "kemijibbola@gmail.com",
                Password = "Pa$$word",
                ConfirmPassword = "Pa$$word",
                Token = generatedToken
            };
        }
    }
}
