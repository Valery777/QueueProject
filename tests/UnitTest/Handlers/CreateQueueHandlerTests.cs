using Microsoft.Extensions.Logging;
using Moq;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Application.Queues.Commands.CreateQueue;
using QueueProject.Domain.Entities; // Make sure to include the correct namespace for Queue
using Xunit;

namespace QueueProject.tests.UnitTest.Handlers
{
    public class CreateQueueHandlerTests
    {
      
        private readonly CreateQueueCommandHandler _handler;
        private readonly Mock<IMongoDbContext> _repo;
        private readonly Mock<ILogger<CreateQueueCommandHandler>> _logger;

        public CreateQueueHandlerTests()
        {
            
            _repo = new Mock<IMongoDbContext>();
            _logger = new Mock<ILogger<CreateQueueCommandHandler>>();
            _handler = new CreateQueueCommandHandler(_repo.Object, _logger.Object);

        }

        [Fact]
        public async Task Handle_ShouldCreateQueue()
        {
            
            var command = new CreateQueueCommand { Name = "Test", Description = "Desc A" };

            // Setup the Queues collection mock to allow InsertOneAsync or similar
            var mockCollection = new Mock<MongoDB.Driver.IMongoCollection<Queue>>();
            _repo.Setup(r => r.Queues).Returns(mockCollection.Object);
            mockCollection
                .Setup(c => c.InsertOneAsync(It.IsAny<Queue>(), null, default))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, default);

            Assert.False(string.IsNullOrEmpty(result));
            mockCollection.Verify(c => c.InsertOneAsync(It.IsAny<Queue>(), null, default), Times.Once);
        }
    }
}
