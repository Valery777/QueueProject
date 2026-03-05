using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using QueueProject.Application.Queues.Commands.CreateQueue;
using QueueProject.Application.Queues.Commands.DeleteQueue;
using QueueProject.Application.Queues.Commands.UpdateQueue;
using QueueProject.Application.Queues.Queries.GetQueueById;
using QueueProject.Application.Queues.Queries.GetQueuesQuery;
using QueueProject.Controllers;
using QueueProject.Domain.Entities;
using Xunit;

namespace QueueProject.tests.UnitTest.Controllers
{
    public class QueuesControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<QueuesController>> _logger;
        private readonly QueuesController _controller;

        public QueuesControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<QueuesController>>();
            _controller = new QueuesController(_mediator.Object, _logger.Object);
        }

        [Fact]
        public async Task GetQueues_ReturnsOk()
        {
            var expected = new List<Queue>
            {
                new Queue { Id = "1", Name = "A", Description = "Desc A", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Queue { Id = "2", Name = "B", Description = "Desc B", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            _mediator.Setup(m => m.Send(It.IsAny<GetQueuesQuery>(), default))
                     .ReturnsAsync(expected);

            var result = await _controller.GetQueues();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, ok.Value);
        }

        [Fact]
        public async Task GetQueueById_ReturnsNotFound()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetQueueById>(), default))
                     .ReturnsAsync((Queue?)null);

            var result = await _controller.GetQueueById("1");

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task CreateQueue_ReturnsCreated()
        {
            var command = new CreateQueueCommand { Name = "Test" };

            _mediator.Setup(m => m.Send(
                    It.IsAny<CreateQueueCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync("123");

            var result = await _controller.CreateQueue(command);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("123", created.RouteValues["id"]);
        }
        [Fact]
        public async Task CreateQueue_ReturnsCreatedAtAction()
        {
            // Arrange
            var command = new CreateQueueCommand { Name = "Test Queue" };

            _mediator.Setup(m => m.Send(
                    It.IsAny<CreateQueueCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync("123"); // returned ID

            // Act
            var result = await _controller.CreateQueue(command);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetQueueById", created.ActionName);
            Assert.Equal("123", created.RouteValues["id"]);
            Assert.Equal(command, created.Value);
        }
        [Fact]
        public async Task CreateQueue_WhenException_Returns500()
        {
            var command = new CreateQueueCommand { Name = "Test" };

            _mediator.Setup(m => m.Send(
                    It.IsAny<CreateQueueCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.CreateQueue(command);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, obj.StatusCode);
        }

        [Fact]
        public async Task UpdateQueue_IdMismatch_ReturnsBadRequest()
        {
            var command = new UpdateQueueCommand { Id = "2", Name = "Test" };

            var result = await _controller.UpdateQueue("1", command);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", bad.Value);
        }
        [Fact]
        public async Task UpdateQueue_ReturnsNotFound_WhenUpdateFails()
        {
            var command = new UpdateQueueCommand { Id = "1", Name = "Updated" };

            _mediator.Setup(m => m.Send(
                    It.IsAny<UpdateQueueCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _controller.UpdateQueue("1", command);

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task UpdateQueue_WhenException_Returns500()
        {
            var command = new UpdateQueueCommand { Id = "1", Name = "Updated" };

            _mediator.Setup(m => m.Send(
                    It.IsAny<UpdateQueueCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Update failed"));

            var result = await _controller.UpdateQueue("1", command);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, obj.StatusCode);
        }

        [Fact]
        public async Task DeleteQueue_ReturnsNoContent_WhenDeleted()
        {
            _mediator.Setup(m => m.Send(
                    It.IsAny<DeleteQueueCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _controller.DeleteQueue("1");

            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task DeleteQueue_ReturnsNotFound_WhenNotDeleted()
        {
            _mediator.Setup(m => m.Send(
                    It.IsAny<DeleteQueueCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _controller.DeleteQueue("1");

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task DeleteQueue_WhenException_Returns500()
        {
            _mediator.Setup(m => m.Send(
                    It.IsAny<DeleteQueueCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Delete failed"));

            var result = await _controller.DeleteQueue("1");

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, obj.StatusCode);
        }




    }
}