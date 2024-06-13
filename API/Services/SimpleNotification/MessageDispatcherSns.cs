using API.Services.SimpleNotification.Interfaces;
using API.Services.SimpleNotification.Models;

namespace API.Services.SimpleNotification
{
    public class MessageDispatcherSns : IMessageDispatcherSns
    {
        public void DispatchMessages(string topicArn, IList<Message> messages, string deduplicationId = null)
        {
            _ = topicArn ?? throw new ArgumentException("Topic Arn Cannot be empty or null", nameof(topicArn));

            throw new NotImplementedException();
        }
    }
}