using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using API.Logging;
using API.Services.SimpleNotification.Models;

namespace API.Services.SimpleNotification
{
    public class MessageDispatcherSns(IServiceProvider serviceProvider) : IMessageDispatcherSns
    {
        private readonly ILogger<MessageAttributeValue> _logger = serviceProvider.GetRequiredService<ILogger<MessageAttributeValue>>();
        private readonly IAmazonSimpleNotificationService _snsClient = serviceProvider.GetRequiredService<IAmazonSimpleNotificationService>();

        public void DispatchMessages(string topicArn, IList<Message> messages, string? deduplicationId = null)
        {
            _ = topicArn ?? throw new ArgumentException("Topic Arn Cannot be empty or null", nameof(topicArn));
            _ = messages ?? throw new ArgumentException("Messages Cannot be empty or null", nameof(messages));

            IList<Task<PublishResponse>> publishTasks = [];
            foreach (var message in messages)
            {
                var publishRequest = new PublishRequest
                {
                    Message = message.Body,
                    Subject = message.Subject,
                    TopicArn = topicArn,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>()
                };

                if (message.IsInfo)
                {
                    publishRequest.MessageDeduplicationId = message.deduplicationId;
                    publishRequest.MessageGroupId = message.MessageGroupId;
                }

                if (message.MessageAttributes != null)
                    foreach (var (key, value) in message.MessageAttributes)
                    {
                        publishRequest.MessageAttributes.Add(key, new MessageAttributeValue { DataType = "String", StringValue = value });
                    }

                _logger.LogAppInfo($"Publishing message with ID {message.Subject} to topic {topicArn}");
                publishTasks.Add(_snsClient.PublishAsync(publishRequest));

            }

            Task.WaitAll([.. publishTasks]);
            foreach (var task in publishTasks)
            {
                if (task.IsFaulted)
                {
                    string message = $"Error has been reported while publishing message {task.Exception}";
                    _logger.LogAppError(task.Exception, message);
                    throw new Exception(message, task.Exception);
                }

            }
        }
    }
}