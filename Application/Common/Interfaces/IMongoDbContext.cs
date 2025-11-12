using MongoDB.Driver;
using QueueProject.Domain.Entities;

namespace QueueProject.Application.Common.Interfaces
{
     public interface IMongoDbContext {IMongoCollection<Queue> Queues { get; }}
}
