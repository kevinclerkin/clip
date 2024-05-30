using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Rembg;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FakeItEasy;

namespace ClipAPITests
{
    public class ProcessImageTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProcessImageTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var removerFake = A.Fake<Remover>();
                    //A.CallTo(() => removerFake.RemoveBackground(A<Stream>.Ignored))
                      //.ReturnsLazily((Stream input) => new MemoryStream());

                    services.AddScoped(_ => removerFake);
                });
            });
        }

        [Fact]
        public async Task ProcessImage_ReturnsBadRequest_WhenNoFileProvided()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/process-image", null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ProcessImage_ReturnsOk_WithProcessedImage()
        {
            // Arrange
            var client = _factory.CreateClient();
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync("test-image.png"));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
            content.Add(fileContent, "file", "test-image.png");

            // Act
            var response = await client.PostAsync("/process-image", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("image/png", response.Content.Headers.ContentType!.ToString());
        }
    }
}