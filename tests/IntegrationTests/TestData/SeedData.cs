using QueueProject.Domain.Entities;
using QueueProject.Infrastructure.Persistence;
using System;

namespace QueueProject.tests.IntegrationTests.TestData
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext db)
        {
            db.Queues.Add(new Queue
            {
                Id = "1",
                Name = "Seed Queue",
                Description= "This is a test Description"
            });

            db.SaveChanges();
        }
    }
}