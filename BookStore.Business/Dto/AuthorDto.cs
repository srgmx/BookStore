using System;

namespace BookStore.Business.Dto
{
    public class AuthorDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PenName { get; set; }
    }
}