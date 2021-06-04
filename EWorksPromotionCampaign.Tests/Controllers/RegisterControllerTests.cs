using EWorksPromotionCampaign.Service;
using EWorksPromotionCampaign.Tests.Helpers;
using EWorksPromotionCampaign.Tests.Models;
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
    public class RegisterControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public RegisterControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/");

            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task Post_WithValidUser_ReturnsCreatedResult()
        {
            var content = GetValidRegisterJsonContent();
            var response = await _client.PostAsync("register", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        //[Theory]
        //[MemberData(nameof(GetInvalidRegisterInputs))]
        //public async Task Post_WithoutName_ReturnsBadRequest(TestRegisterInputModel registerInputModel)
        //{
        //    var response = await _client.PostAsJsonAsync("register", registerInputModel, JsonSerializerHelper.DefaultSerialisationOptions);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //}
        private static JsonContent GetValidRegisterJsonContent()
        {
            return JsonContent.Create(GetValidRegisterInputModel());
        }
        public static IEnumerable<object[]> GetInvalidRegisterInputs()
        {
            return GetInvalidInputsAndProblemDetailsErrorValidator().Select(x => new[] { x[0] });
        }
        private static TestRegisterInputModel GetValidRegisterInputModel()
        {
            return new TestRegisterInputModel
            {
                FirstName = "Oluwakemi",
                LastName = "Awosile",
                MiddleName = "",
                Phone = "2348080747474",
                Password = "Password",
                ConfirmPassword = "Password",
                Email = "kemijibola@yahoo.com",
                Address = "Ibadan,Oyo-State",
                DateOfBirth = DateTime.Parse("1940/02/03")
            };
        }
        public static IEnumerable<object[]> GetInvalidInputsAndProblemDetailsErrorValidator()
        {
            var testData = new List<object[]>
            {
                new object[]
                {
                    GetValidRegisterInputModel().CloneWith(x => x.FirstName = null),
                    new Action<KeyValuePair<string, string[]>>(kvp =>
                    {
                        Assert.Equal("FirstName", kvp.Key);
                        var error = Assert.Single(kvp.Value);
                        Assert.StartsWith("The FirstName field is required.", error);
                    })
                },

                new object[]
                {
                    GetValidRegisterInputModel().CloneWith(x => x.LastName = null),
                    new Action<KeyValuePair<string, string[]>>(kvp =>
                    {
                        Assert.Equal("LastName", kvp.Key);
                        var error = Assert.Single(kvp.Value);
                        Assert.Equal("The LastName field is required.", error);
                    })
                },

                new object[]
                {
                    GetValidRegisterInputModel().CloneWith(x => x.Phone =  null),
                    new Action<KeyValuePair<string, string[]>>(kvp =>
                    {
                        Assert.Equal("Phone", kvp.Key);
                        var error = Assert.Single(kvp.Value);
                        Assert.Equal("The Phone field is required.", error);
                    })
                },
                new object[]
                {
                    GetValidRegisterInputModel().CloneWith(x => x.Password =  null),
                    new Action<KeyValuePair<string, string[]>>(kvp =>
                    {
                         Assert.Equal("Password", kvp.Key);
                        var error = Assert.Single(kvp.Value);
                        Assert.Equal("The Password field is required.", error);
                    })
                },
                new object[]
                {
                    GetValidRegisterInputModel().CloneWith(x => x.Email = "NOT A VALID EMAIL"),
                    new Action<KeyValuePair<string, string[]>>(kvp =>
                    {
                         Assert.Equal("Email", kvp.Key);
                        var error = Assert.Single(kvp.Value);
                        Assert.Equal("Provided Email is not valid.", error);
                    })
                }
                //new object[]
                //{
                //    GetValidRegisterInputModel().CloneWith(x => x.DateOfBirth = default),
                //    new Action<KeyValuePair<string, string[]>>(kvp =>
                //    {
                //         Assert.Equal("DateOfBirth", kvp.Key);
                //        var error = Assert.Single(kvp.Value);
                //        Assert.Equal("The DateOfBirth field is required.", error);
                //    })
                //}
                //new object[]
                //{
                //    GetValidRegisterInputModel().CloneWith(x => x.Address = null),
                //    new Action<KeyValuePair<string, string[]>>(kvp =>
                //    {
                //         Assert.Equal("Address", kvp.Key);
                //        var error = Assert.Single(kvp.Value);
                //        Assert.Equal("The Address field is required.", error);
                //    })
                //}
            };

            return testData;
        }
    }
}
