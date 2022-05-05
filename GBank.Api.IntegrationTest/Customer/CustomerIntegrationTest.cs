using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using GBank.Api.Application.Customers.Commands;
using GBank.Api.Models;

namespace GBank.Api.IntegrationTest.Customer
{
    public class CustomerIntegrationTest
    {
        [Fact]
        public async Task Register_Customer_Should_Return_UnAuthorized()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;
            var customer = new Domain.Documents.Customer()
            {
                Name = "Berkay",
                Address = "IST",
                Email = "test@mail.com"
            };

            // Act
            var response = await httpClient.PostAsync($"/api/customers", new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Register_Customer_Should_Return_BadRequest()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;

            var command = new RegisterCustomerCommand
            {
                Name = "Berkay",
                Address = "IST"
            };

            var json = JsonConvert.SerializeObject(command);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.PostAsync($"/api/customers", data);
            var customerError = JsonConvert.DeserializeObject<HttpResponseBase>(await response.Content.ReadAsStringAsync()).Error;

            // Assert
            customerError.Exception.Should().NotBeNullOrEmpty();
            customerError.Code.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_Customer_Should_Return_CustomerId()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            var tokenResponse = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await tokenResponse.Content.ReadAsStringAsync()).Data;

            var command = new RegisterCustomerCommand
            {
                Name = "Berkay",
                Address = "IST",
                Email = "tst@mail.com"
            };

            var json = JsonConvert.SerializeObject(command);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await httpClient.PostAsync($"/api/customers", data);
            var customer = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await response.Content.ReadAsStringAsync()).Data;

            // Assert
            customer.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
