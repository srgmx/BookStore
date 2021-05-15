using BookCoreLibrary.EventBus.Core;
using System;
using System.Collections.Generic;

namespace BookStore.Domain.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public List<ReservedBook> ReservedBooks { get; set; }
    }

    public class ReservedBook
    {
        public Guid BookId { get; set; }

        public int Count { get; set; }
    }
}
