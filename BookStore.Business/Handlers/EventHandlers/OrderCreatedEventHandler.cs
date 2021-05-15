using BookCoreLibrary.EventBus.Core;
using BookStore.Data.Abstraction;
using BookStore.Domain.Commands;
using BookStore.Domain.Events;
using System.Threading.Tasks;

namespace BookStore.Business.Handlers.EventHandlers
{
    public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public OrderCreatedEventHandler(
            IUnitOfWork unitOfWork,
            IEventBus eventBus
        )
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }

        public async Task HandleAsync(OrderCreatedEvent @event)
        {
            // TODO: Implememt batch update and concurency handling later

            foreach (var book in @event.ReservedBooks)
            {
                var bookInDb = await _unitOfWork.BookRepository.GetBookByIdAsync(book.Id);
                bookInDb.ReservedQuantity += book.Count;
                await _unitOfWork.BookRepository.UpdateAsync(bookInDb);
            }
            await _unitOfWork.SaveAsync();

            var command = new AckOrderReservedCommand
            {
                OrderId = @event.OrderId,
                IsSuccess = true
            };
            await _eventBus.SendCommandAsync(command);
        }
    }
}
