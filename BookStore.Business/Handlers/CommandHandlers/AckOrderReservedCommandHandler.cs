using AutoMapper;
using BookCoreLibrary.EventBus.Core;
using BookCoreLibrary.EventBus.RabbitMq;
using BookStore.Domain.Commands;
using BookStore.Domain.Configuration;
using BookStore.Domain.Events;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Business.Handlers.CommandHandlers
{
    public class AckOrderReservedCommandHandler : ICommandHandler<AckOrderReservedCommand, RabbitMqTopicDestination>
    {
        private readonly IEventBus _eventBus;
        private readonly RabbitMqPublishingConfiguration _destinations;
        private readonly IMapper _mapper;

        public AckOrderReservedCommandHandler(
            IEventBus eventBus,
            IOptions<RabbitMqPublishingConfiguration> destinations,
            IMapper mapper
        )
        {
            _eventBus = eventBus;
            _destinations = destinations.Value;
            _mapper = mapper;
        }

        public RabbitMqTopicDestination Destination
        {
            get
            {
                return new RabbitMqTopicDestination()
                {
                    TopicExchange = _destinations.OrderReservedTopic.TopicExchange,
                    RoutingKey = _destinations.OrderReservedTopic.RoutingKey
                };
            }
        }

        public async Task<bool> Handle(AckOrderReservedCommand request, CancellationToken cancellationToken)
        {
            var @event = _mapper.Map<AckOrderReservedCommand, OrderReservedEvent>(request);
            await _eventBus.PublishEventAsync(@event, Destination);

            return true;
        }
    }
}
