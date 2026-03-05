using QueueProject.Application.Common.Interfaces;
using QueueProject.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueueProject.Application.Common
{
    public class InMemoryQueueRepository : IQueueRepository
    {
        private readonly List<Queue> _data = new();

        public Task<IEnumerable<Queue>> GetAllAsync() =>
            Task.FromResult(_data.AsEnumerable());

        public Task<Queue?> GetByIdAsync(string id) =>
            Task.FromResult(_data.FirstOrDefault(x => x.Id == id));

        public Task CreateAsync(Queue queue)
        {
            _data.Add(queue);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Queue queue)
        {
            var index = _data.FindIndex(x => x.Id == queue.Id);
            if (index >= 0) _data[index] = queue;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            _data.RemoveAll(x => x.Id == id);
            return Task.CompletedTask;
        }
    }
}


