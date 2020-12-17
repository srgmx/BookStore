using System;

namespace BookStore.Business.Dto
{
    public class AuthorBookDto
    {
        public Guid BookId { get; set; }

        public string BookName { get; set; }
    }
}
