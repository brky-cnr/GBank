using FluentAssertions;
using Newtonsoft.Json;
using GBank.Api.Application.Transactions.Commands;
using GBank.Api.Application.Transactions.Queries;
using GBank.Api.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GBank.Api.IntegrationTest.Transaction
{
    public class TransactionIntegrationTest
    {
        [Fact]
        public async Task Place_Account_Transaction_Should_Return_BadRequest()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;

            var command = new PlaceAccountTransactionCommand
            {
                CustomerId = "123",
                AccountId = "123",
                Amount = 100,
                Description = "Test",
                IsDeposit = true
            };

            var json = JsonConvert.SerializeObject(command);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.PostAsync($"/api/transactions", data);
            var accountTransactionError = JsonConvert.DeserializeObject<HttpResponseBase>(await response.Content.ReadAsStringAsync()).Error;

            // Assert
            accountTransactionError.Exception.Should().NotBeNullOrEmpty();
            accountTransactionError.Code.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Place_Account_Transaction_Should_Return_UnAuthorized()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var command = new PlaceAccountTransactionCommand
            {
                CustomerId = "626f1f40c222ea768c5a688c",
                AccountId = "626f22f47f61bf7d0c18bd0e",
                Amount = 100,
                Description = "Test",
                IsDeposit = true
            };

            var json = JsonConvert.SerializeObject(command);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await httpClient.PostAsync($"/api/transactions", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }
        [Fact]
        public async Task Place_Account_Transaction_Should_Return_TransactionId()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;

            var command = new PlaceAccountTransactionCommand
            {
                CustomerId = "626f1f40c222ea768c5a688c",
                AccountId = "626f22f47f61bf7d0c18bd0e",
                Amount = 100,
                Description = "Test",
                IsDeposit = true
            };

            var json = JsonConvert.SerializeObject(command);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.PostAsync($"/api/transactions", data);
            var accountTransaction = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await response.Content.ReadAsStringAsync()).Data;

            // Assert
            accountTransaction.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Get_Customer_Transactions_Should_Return_Customer_Transactions()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;
            var customerId = "626f1f40c222ea768c5a688c";

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.GetAsync($"/api/transactions/customers/{customerId}");
            var customerTransacitons = JsonConvert.DeserializeObject<HttpResponseBase<TransactionDTO>>(await response.Content.ReadAsStringAsync()).Data;

            // Assert
            customerTransacitons.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Customer_Transactions_Should_Return_BadRequest()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;
            var customerId = "123";

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.GetAsync($"/api/transactions/customers/{customerId}");
            var customerTransacitonsError = JsonConvert.DeserializeObject<HttpResponseBase>(await response.Content.ReadAsStringAsync()).Error;

            // Assert
            customerTransacitonsError.Exception.Should().NotBeNullOrEmpty();
            customerTransacitonsError.Code.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_Customer_Transactions_ByDate_Should_Return_Customer_Transactions()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;
            var customerId = "626f1f40c222ea768c5a688c";
            var startDate = "2022-04-02T10:30:05.542Z";
            var endDate = "2022-06-02T10:30:05.542Z";

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.GetAsync($"/api/transactions/customers/{customerId}/{startDate}/{endDate}");
            var customerTransacitons = JsonConvert.DeserializeObject<HttpResponseBase<TransactionDTO>>(await response.Content.ReadAsStringAsync()).Data;

            // Assert
            customerTransacitons.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Customer_Transactions_ByDate_Should_Return_BadRequest()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;
            var customerId = "626f1f40c222ea768c5a688c";
            var startDate = "2023-04-02T10:30:05.542Z";
            var endDate = "2022-06-02T10:30:05.542Z";

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.GetAsync($"/api/transactions/customers/{customerId}/{startDate}/{endDate}");
            var customerTransacitonsError = JsonConvert.DeserializeObject<HttpResponseBase>(await response.Content.ReadAsStringAsync()).Error;

            // Assert
            customerTransacitonsError.Exception.Should().NotBeNullOrEmpty();
            customerTransacitonsError.Code.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
