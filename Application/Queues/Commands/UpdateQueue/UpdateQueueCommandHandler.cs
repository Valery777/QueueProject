using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Application.Queues.Commands.CreateQueue;
using System.Net.NetworkInformation;

namespace QueueProject.Application.Queues.Commands.UpdateQueue
{
    public class UpdateQueueCommandHandler:IRequestHandler<UpdateQueueCommand, bool>
    {
        
        private readonly IMongoDbContext _context;
        private readonly ILogger<CreateQueueCommandHandler> _logger;

        /*      SOLID Principles:
           Single Responsibility(each handler does one thing)
           Using Dependency Injection to get the MongoDB context and Logger
        */
        public UpdateQueueCommandHandler(IMongoDbContext context,
            ILogger<CreateQueueCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateQueueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = Builders<Domain.Entities.Queue>.Filter.Eq(q => q.Id, request.Id);
                var update = Builders<Domain.Entities.Queue>.Update
                    .Set(q => q.Name, request.Name)
                    .Set(q => q.Description, request.Description)
                    .Set(q => q.UpdatedAt, DateTime.UtcNow);

                var result = await _context.Queues.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
                return result.ModifiedCount > 0;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to update queue with name: {request.Name}. Exception {ex.Message}");
                return false;
            }
        }

    }
}
