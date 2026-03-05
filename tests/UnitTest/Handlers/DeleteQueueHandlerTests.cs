using MediatR;
using Moq;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Application.Queues.Commands.DeleteQueue;
using QueueProject.Application.Queues.Commands.UpdateQueue;
using System.Threading;
using Xunit;

public class DeleteQueueHandlerTests 
{
    private readonly Mock<IQueueRepository> _repo;
    private readonly Mock<IMongoDbContext> _context;
    private readonly Mock<ILogger<DeleteQueueCommandHandler>> _logger;

    public DeleteQueueHandlerTests()
    {
        _repo = QueueRepositoryMock.GetMock();
        _context = new Mock<IMongoDbContext>();
        _logger = new Mock<ILogger<DeleteQueueCommandHandler>>(); // Fix logger type
    }

    [Fact]
    public async Task Handle_ReturnsTrue_WhenDeleted()
    {
       var handler = new DeleteQueueCommandHandler(_context.Object, _logger.Object);
      
        var result = await handler.Handle(
            new DeleteQueueCommand { Id = "1" }, default);

        Assert.True(result);
    }
}