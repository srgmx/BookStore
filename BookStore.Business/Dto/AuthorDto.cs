using System;
using System.Collections.Generic;

namespace BookStore.Business.Dto
{
    public class AuthorDto
    {
        public AuthorDto()
        {
            AuthorBooks = new List<AuthorBookDto>();
        }

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PenName { get; set; }

        public List<AuthorBookDto> AuthorBooks { get; set; }
    }
}
