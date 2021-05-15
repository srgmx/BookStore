using BookCoreLibrary.EventBus.Core;
using BookCoreLibrary.EventBus.RabbitMq;
using BookStore.Business.Handlers.EventHandlers;
using BookStore.Domain.Configuration;
using BookStore.Domain.Events;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookStore.Dependencies.Extensions
{
    public static class EventBusExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration config)
        {
            // Commands
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            // Configurations
            services.Configure<RabbitMqConnectionConfiguration>(config.GetSection("RabbitMqConnectionConfiguration"));
            services.Configure<RabbitMqPublishingConfiguration>(config.GetSection("RabbitMqDestinationConfiguration:Publishing"));
            services.Configure<RabbitMqConsumingConfiguration>(config.GetSection("RabbitMqDestinationConfiguration:Consuming"));

            // Event Handlers
            services.AddTransient(typeof(IEventHandler<OrderCreatedEvent>), typeof(OrderCreatedEventHandler));

            // Event Bus
            services.AddRabbitMqEventBus();

            return services;
        }

        public static IEventBus ConfigureSubscriptions(
            this IEventBus eventBus,
            RabbitMqConsumingConfiguration queueDestinations
        )
        {
            var orderCreatedDestination = new RabbitMqQueueDestination { Queue = queueDestinations.OrderCreatedQueue };

            eventBus
                .Subscribe<OrderCreatedEvent, OrderCreatedEventHandler, RabbitMqQueueDestination>(orderCreatedDestination)
                .ConsumeEvents();

            return eventBus;
        }
    }
}
