using System;

namespace BookStore.Business.Dto
{
    public class AuthorToUpdateDto
    {
        public Guid Id { get; set; }

        public string PenName { get; set; }
    }
}