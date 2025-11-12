using MediatR;
using QueueProject.Domain.Entities;
namespace QueueProject.Application.Queues.Queries.GetQueueById
{
    //CQRS Pattern: Separated commands(write operations) and queries(read operations)
    public class GetQueueById : IRequest<Queue>
    {
        public string Id { get; set; }
    }
}
