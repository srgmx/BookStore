using System;
using System.Collections.Generic;

namespace BookStore.Business.Dto
{
    public class BookToAddDto
    {
        public string Name { get; set; }

        public string Articul { get; set; }

        public int AvailableQuantity { get; set; }

        public decimal Price { get; set; }

        public DateTime PublishedAt { get; set; }

        public IEnumerable<Guid> AuthorIds { get; set; }
    }
}
