using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using GBank.Api.Application.Accounts.Commands;
using GBank.Api.Application.Accounts.Queries;
using GBank.Api.Models;

namespace GBank.Api.IntegrationTest.Account
{
    public class AccountIntegrationTest
    {
        [Fact]
        public async Task Register_Account_Should_Return_UnAuthorized()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var command = new RegisterAccountCommand
            {
                CustomerId = "626dbc01ff083a886b8c73b1",
                Balance = 100
            };

            // Act
            var response = await httpClient.PostAsync($"/api/accounts", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Register_Account_Should_Return_BadRequest()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;

            var command = new RegisterAccountCommand
            {
                CustomerId = "626dbc01ff083a886b8c73b1",
                Balance = 100
            };

            var json = JsonConvert.SerializeObject(command);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.PostAsync($"/api/accounts", data);
            var accountError = JsonConvert.DeserializeObject<HttpResponseBase>(await response.Content.ReadAsStringAsync()).Error;

            // Assert
            accountError.Exception.Should().NotBeNullOrEmpty();
            accountError.Code.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_Account_Should_Return_AccountId()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;

            var command = new RegisterAccountCommand
            {
                CustomerId = "626f1f40c222ea768c5a688c",
                Balance = 100
            };

            var json = JsonConvert.SerializeObject(command);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.PostAsync($"/api/accounts", data);
            var account = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await response.Content.ReadAsStringAsync()).Data;

            // Assert
            account.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Get_Account_Should_Return_Account()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;
            var accountId = "626f1f40c222ea768c5a688d";

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.GetAsync($"/api/accounts/{accountId}");
            var account = JsonConvert.DeserializeObject<HttpResponseBase<AccountDTO>>(await response.Content.ReadAsStringAsync()).Data;

            // Assert
            account.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Account_Should_Return_BadRequest()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;
            var accountId = "asd";

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.GetAsync($"/api/accounts/{accountId}");
            var accountError = JsonConvert.DeserializeObject<HttpResponseBase>(await response.Content.ReadAsStringAsync()).Error;

            // Assert
            accountError.Should().NotBeNull();
            accountError.Code.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
