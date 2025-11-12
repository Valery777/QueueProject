
using MediatR;
using QueueProject.Application.Common.Interfaces;
namespace QueueProject.Application.Queues.Commands.CreateQueue
{
    //CQRS Pattern: Separated commands(write operations) and queries(read operations)
    public class CreateQueueCommand:IRequest<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        

    }
}
