using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Application.Queues.Commands.CreateQueue;
using QueueProject.Domain.Entities;
using QueueProject.Infrastructure.Settings;
namespace QueueProject.Infrastructure.Persistence
{
    public class MongoDbContext: IMongoDbContext
    {
        //MongoDB Integration: Full CRUD operations with proper repository pattern
        //Clean Architecture: Separation of concerns across layers
      
        private readonly IMongoDatabase _database;
        private readonly MongoDbSettings _settings;

        public MongoDbContext(IOptions<MongoDbSettings> settings, 
            ILogger<CreateQueueCommandHandler> logger)
        {
            try
            {
                _settings = settings.Value;
                var client = new MongoClient(_settings.ConnectionString);
                _database = client.GetDatabase(_settings.DatabaseName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Could not connect to MongoDB. Exception {ex.Message}");
                throw new Exception("Could not connect to MongoDB", ex);
            }
        }

        public IMongoCollection<Queue> Queues => _database.GetCollection<Queue>("queues");

    }
}
