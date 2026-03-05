using Microsoft.AspNetCore.Mvc;
using QueueProject.Application.Queues.Commands.UpdateQueue;
using QueueProject.tests.IntegrationTests.Factory;
using System.Net;
using Xunit;

public class UpdateQueue : IClassFixture<QueueApiFactory>
{
    private readonly HttpClient _client;

    public UpdateQueue(QueueApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateQueue_ReturnsNoContent()
    {
        var command = new { Id = "1", Name = "Updated" };
        var content = JsonContent.Create(command);

        var response = await _client.PutAsync("/api/queues/1", content);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    


}

