using QueueProject.Domain.Entities;
namespace QueueProject.Application.Common.Interfaces
{
    public interface IQueueRepository
    {
            Task<Queue> GetByIdAsync(string id);
            Task<IEnumerable<Queue>> GetAllAsync();
            Task CreateAsync(Queue queue);
            Task UpdateAsync(Queue queue);
            Task DeleteAsync(string id);
    }
}
