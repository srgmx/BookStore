using BookCoreLibrary.EventBus.Core;
using System;

namespace BookStore.Domain.Events
{
    public class OrderReservedEvent : BaseEvent
    {
        public Guid OrderId { get; set; }

        public bool IsSuccess { get; set; }
    }
}
