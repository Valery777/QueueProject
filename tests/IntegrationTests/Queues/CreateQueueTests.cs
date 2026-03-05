using QueueProject.tests.IntegrationTests.Factory;
using System.Net;
using Xunit;

public class CreateQueuesTests : IClassFixture<QueueApiFactory>
{
    private readonly HttpClient _client;

    public CreateQueuesTests(QueueApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateQueue_ReturnsCreated()
    {
        var command = new { Name = "New Queue" };
        var content = JsonContent.Create(command);

        var response = await _client.PostAsync("/api/queues", content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}

