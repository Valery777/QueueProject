using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using QueueProject.Application.Common.Interfaces;
namespace QueueProject.Application.Queues.Commands.CreateQueue
{
    public class CreateQueueCommandHandler : IRequestHandler<CreateQueueCommand, string>
    {
        private readonly IMongoDbContext _context;
        private readonly ILogger<CreateQueueCommandHandler> _logger;
      
        /*      SOLID Principles:
           Single Responsibility(each handler does one thing)
           Using Dependency Injection to get the MongoDB context and Logger
        */
        public CreateQueueCommandHandler(IMongoDbContext context,
            ILogger<CreateQueueCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<string> Handle(CreateQueueCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var queue = new Domain.Entities.Queue
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Queues.InsertOneAsync(queue, cancellationToken: cancellationToken);
                return queue.Id;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to create queue with name: {request.Name}. Exception {ex.Message}");
                throw new Exception($"Failed to create queue with name: {request.Name}", ex);

            }
        }

    }
}
