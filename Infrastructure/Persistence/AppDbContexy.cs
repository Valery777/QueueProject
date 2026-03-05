using Microsoft.EntityFrameworkCore;
using QueueProject.Domain.Entities;

namespace QueueProject.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Queue> Queues => Set<Queue>();
    }
}

