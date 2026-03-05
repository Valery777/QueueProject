using Moq;
using QueueProject.Application.Common.Interfaces;
public static class QueueRepositoryMock
{
    public static Mock<IQueueRepository> GetMock()
    {
        return new Mock<IQueueRepository>();
    }
}