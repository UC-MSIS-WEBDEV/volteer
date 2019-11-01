using System.Threading.Tasks;

namespace Vt.Platform.Domain.Messaging
{
    public interface IQueueManager
    {
        Task BuildQueues();
    }
}