using System;
using System.Collections.Generic;

namespace BookStore.Business.Dto
{
    public class BookToAddDto
    {
        public string Name { get; set; }

        public IEnumerable<Guid> AuthorIds { get; set; }
    }
}
