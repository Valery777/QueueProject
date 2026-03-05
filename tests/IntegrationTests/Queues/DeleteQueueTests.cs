using QueueProject.tests.IntegrationTests.Factory;
using System.Net;
using Xunit;

public class DeleteQueue : IClassFixture<QueueApiFactory>
{
    private readonly HttpClient _client;

    public DeleteQueue(QueueApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DeleteQueue_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync("/api/queues/1");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }


}

