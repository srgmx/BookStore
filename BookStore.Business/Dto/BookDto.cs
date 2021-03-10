using System;
using System.Collections.Generic;

namespace BookStore.Business.Dto
{
    public class BookDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Articul { get; set; }

        public int AvailableQuantity { get; set; }

        public decimal Price { get; set; }

        public string PublishedAt { get; set; }

        public IEnumerable<BookAuthorDto> Authors { get; set; }
    }
}
