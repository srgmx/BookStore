namespace BookStore.Domain.Configuration
{
    /// <summary>
    /// Rabbit MQ configuration for consuming events (messages)
    /// </summary>
    public class RabbitMqConsumingConfiguration
    {
        public string OrderCreatedQueue { get; set; }
    }
}
