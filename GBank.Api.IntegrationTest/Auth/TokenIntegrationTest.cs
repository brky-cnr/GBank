using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using GBank.Api.Models;

namespace GBank.Api.IntegrationTest.Auth
{
    public class TokenIntegrationTest
    {
        [Fact]
        public async Task Should_Return_Token()
        {
            // Arrange
            using var httpClient = new TestClientProvider().HttpClient;

            // Act
            var response = await httpClient.PostAsync($"/api/auth/token", new StringContent("{}", Encoding.UTF8, "application/json"));
            var token = JsonConvert.DeserializeObject<HttpResponseBase<string>>(await response.Content.ReadAsStringAsync()).Data;

            // Assert
            token.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
