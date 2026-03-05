using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QueueProject.Application.Common.Interfaces;
using QueueProject.Infrastructure.Persistence;   // AppDbContext
using QueueProject.tests.IntegrationTests.TestData;
using System.Linq;
using QueueProject.Application.Common; // IQueueRepository
namespace QueueProject.tests.IntegrationTests.Factory
{
    public class QueueApiFactory : WebApplicationFactory<ProgramVisibility>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove MongoDB context
                var mongoDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IMongoDbContext));

                if (mongoDescriptor != null)
                    services.Remove(mongoDescriptor);

                // Remove repository
                var repoDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IQueueRepository));

                if (repoDescriptor != null)
                    services.Remove(repoDescriptor);

                // Add fake in-memory repository for tests
                services.AddSingleton<IQueueRepository, InMemoryQueueRepository>();
            });
        }
    }
}