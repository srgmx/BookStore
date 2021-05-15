using BookCoreLibrary.EventBus.Core;
using System;

namespace BookStore.Domain.Commands
{
    public class AckOrderReservedCommand : BaseCommand
    {
        public Guid OrderId { get; set; }

        public bool IsSuccess { get; set; }
    }
}
