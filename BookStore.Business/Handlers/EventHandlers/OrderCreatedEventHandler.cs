using AutoMapper;
using BookCoreLibrary.EventBus.Core;
using BookStore.Data.Abstraction;
using BookStore.Domain.Commands;
using BookStore.Domain.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BookStore.Business.Handlers.EventHandlers
{
    public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(
            IUnitOfWork unitOfWork,
            IEventBus eventBus,
            IMapper mapper,
            ILogger<OrderCreatedEventHandler> logger
        )
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task HandleAsync(OrderCreatedEvent @event)
        {
            _logger.LogInformation("{EventHandler} {Event} {CorrelationId}. Begin handling.", 
                nameof(OrderCreatedEventHandler), nameof(OrderCreatedEvent), @event.CorrelationId);

            // TODO: Implememt batch update and concurency handling later

            foreach (var book in @event.ReservedBooks)
            {
                var bookInDb = await _unitOfWork.BookRepository.GetBookByIdAsync(book.Id);
                bookInDb.ReservedQuantity += book.Count;
                await _unitOfWork.BookRepository.UpdateAsync(bookInDb);
            }
            await _unitOfWork.SaveAsync();

            var command =  _mapper.Map<OrderCreatedEvent, AckOrderReservedCommand>(@event);
            command.IsSuccess = true;
            await _eventBus.SendCommandAsync(command);

            _logger.LogInformation("{EventHandler} {Event} {CorrelationId}. Finish handling.",
                nameof(OrderCreatedEventHandler), nameof(OrderCreatedEvent), @event.CorrelationId);
        }
    }
}
