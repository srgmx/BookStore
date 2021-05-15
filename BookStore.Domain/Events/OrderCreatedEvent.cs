using BookCoreLibrary.EventBus.Core;
using System;
using System.Collections.Generic;

namespace BookStore.Domain.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public Guid OrderId { get; set; }

        public List<ReservedBook> ReservedBooks { get; set; }
    }

    public class ReservedBook
    {
        public Guid Id { get; set; }
        public int Count { get; set; }
    }
}
