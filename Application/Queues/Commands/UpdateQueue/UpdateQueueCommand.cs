using MediatR;

namespace QueueProject.Application.Queues.Commands.UpdateQueue
{
    //CQRS Pattern: Separated commands(write operations) and queries(read operations)
    public class UpdateQueueCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       
    }
}
