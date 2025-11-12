using MediatR;

namespace QueueProject.Application.Queues.Commands.DeleteQueue
{
    //CQRS Pattern: Separated commands(write operations) and queries(read operations)
    public class DeleteQueueCommand : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
