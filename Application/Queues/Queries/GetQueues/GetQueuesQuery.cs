using MediatR;
using QueueProject.Domain.Entities;
namespace QueueProject.Application.Queues.Queries.GetQueuesQuery
{
    //CQRS Pattern: Separated commands(write operations) and queries(read operations)
    public class GetQueuesQuery : IRequest<List<Queue>>
    {
        public bool? IsActive { get; set; }
    }
}
