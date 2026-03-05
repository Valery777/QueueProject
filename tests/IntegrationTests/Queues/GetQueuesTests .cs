using QueueProject.tests.IntegrationTests.Factory;
using Xunit;

public class GetQueuesTests : IClassFixture<QueueApiFactory>
{
    private readonly HttpClient _client;

    public GetQueuesTests(QueueApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetQueues_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/queues");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("Seed Queue", json);
    }
}