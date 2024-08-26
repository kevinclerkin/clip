using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;
using System.Net.Http.Headers;

namespace ClipAPITests
{
    public class ProcessImageIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProcessImageIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Setup HttpClient for the test
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task ProcessImage_ReturnsBadRequest_WhenNoFilePresent()
        {
            // Arrange
            HttpContent formData = null!;

            // Act
            var response = await _client.PostAsync("/process-image", formData);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

    }
}