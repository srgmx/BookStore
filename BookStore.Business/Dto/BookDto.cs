using System;
using System.Collections.Generic;

namespace BookStore.Business.Dto
{
    public class BookDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<BookAuthorDto> Authors { get; set; }
    }
}
