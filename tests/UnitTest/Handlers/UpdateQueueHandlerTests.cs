using Microsoft.Extensions.Logging;
using Moq;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Application.Queues.Commands.UpdateQueue;
using QueueProject.Domain.Entities; // Make sure to include the correct namespace for Queue
using Xunit;

public class UpdateQueueHandlerTests
{
    private readonly Mock<IQueueRepository> _repo;
    private readonly Mock<IMongoDbContext> _context;
    private readonly Mock<ILogger<UpdateQueueCommandHandler>> _logger;

    public UpdateQueueHandlerTests()
    {
        _repo = QueueRepositoryMock.GetMock();
        _context = new Mock<IMongoDbContext>();
        _logger = new Mock<ILogger<UpdateQueueCommandHandler>>();
    }

    [Fact]
    public async Task Handle_ReturnsTrue_WhenUpdated()
    {
        var queue = new Queue
        {
            Id = "1",
            Name = "A",
            Description = "Desc A",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _repo.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(queue);

        var handler = new UpdateQueueCommandHandler(_context.Object, _logger.Object);

        var result = await handler.Handle(
            new UpdateQueueCommand { Id = "1", Name = "New", Description = "Desc B" }, default);

        Assert.True(result);
    }
}