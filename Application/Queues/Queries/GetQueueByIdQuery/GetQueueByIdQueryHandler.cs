using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Application.Queues.Commands.CreateQueue;
using QueueProject.Domain.Entities;
using System.Net.NetworkInformation;
namespace QueueProject.Application.Queues.Queries.GetQueueById
{
    public class GetQueueByIdQueryHandler : IRequestHandler<GetQueueById, Queue>
    {
        private readonly IMongoDbContext _context;
        private readonly ILogger<CreateQueueCommandHandler> _logger;

        /*      SOLID Principles:
           Single Responsibility(each handler does one thing)
           Using Dependency Injection to get the MongoDB context and Logger
        */
        public GetQueueByIdQueryHandler(IMongoDbContext context,
            ILogger<CreateQueueCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Queue> Handle(GetQueueById request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Queues
                    .Find(q => q.Id == request.Id)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving queue with ID {QueueId}", request.Id);
                throw;
            }
        }

    }
}
