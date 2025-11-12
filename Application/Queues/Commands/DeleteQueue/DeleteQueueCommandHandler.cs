using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Application.Queues.Commands.CreateQueue;
using QueueProject.Domain.Entities;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace QueueProject.Application.Queues.Commands.DeleteQueue
{
    public class DeleteQueueCommandHandler : IRequestHandler<DeleteQueueCommand, bool>
    {
        private readonly IMongoDbContext _context;
        private readonly ILogger<CreateQueueCommandHandler> _logger;

        /*      SOLID Principles:
           Single Responsibility(each handler does one thing)
           Using Dependency Injection to get the MongoDB context and Logger
        */
        public DeleteQueueCommandHandler(IMongoDbContext context, 
            ILogger<CreateQueueCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteQueueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Queues.DeleteOneAsync(q => q.Id == request.Id, cancellationToken);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete queue with name: {request.Id}. Exception {ex.Message}");
                return false;

            }
        }
             
         
    }
}
