using BookCoreLibrary.EventBus.Core;

namespace BookStore.Domain.Configuration
{
    /// <summary>
    /// Rabbit MQ configuration for publishing events (messages)
    /// </summary>
    public class RabbitMqPublishingConfiguration
    {
        public TopicDestinationConfiguration OrderReservedTopic { get; set; }
    }
}
