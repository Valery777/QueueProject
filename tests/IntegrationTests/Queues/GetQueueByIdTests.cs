using QueueProject.tests.IntegrationTests.Factory;
using Xunit;

public class GetQueueByIdTest : IClassFixture<QueueApiFactory>
{
    private readonly HttpClient _client;

    public GetQueueByIdTest(QueueApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetQueueById_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/queues/1");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("Seed Queue", json);
    }
}
