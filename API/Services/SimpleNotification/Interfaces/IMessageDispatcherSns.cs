using API.Services.SimpleNotification.Models;

namespace API.Services.SimpleNotification.Interfaces
{
    public interface IMessageDispatcherSns
    {
        void DispatchMessages(string topicArn, IList<Message> messages, string deduplicationId = null);
    }
}