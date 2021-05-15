using BookCoreLibrary.EventBus.Core;
using BookStore.Domain.Events;
using System.Threading.Tasks;

namespace BookStore.Business.Handlers.EventHandlers
{
    public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        public Task HandleAsync(OrderCreatedEvent @event)
        {
            return Task.Run(() => {; });
        }
    }
}
