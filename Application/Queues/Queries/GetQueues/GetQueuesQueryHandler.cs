using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Application.Queues.Commands.CreateQueue;
using QueueProject.Domain.Entities;

namespace QueueProject.Application.Queues.Queries.GetQueuesQuery
{
    public class GetQueuesQueryHandler : IRequestHandler<GetQueuesQuery, List<Queue>>
    {
        private readonly IMongoDbContext _context;
        private readonly ILogger<CreateQueueCommandHandler> _logger;

        /*      SOLID Principles:
           Single Responsibility(each handler does one thing)
           Using Dependency Injection to get the MongoDB context and Logger
        */
        public GetQueuesQueryHandler(IMongoDbContext context, 
            ILogger<CreateQueueCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Queue>> Handle(GetQueuesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = Builders<Queue>.Filter.Empty;

                return await _context.Queues
                    .Find(filter)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving queues.");
                throw;
            }
        }

    }
}
