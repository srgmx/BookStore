using System;

namespace BookStore.Business.Dto
{
    public class AuthorToAddDto
    {
        public Guid UserId { get; set; }

        public string PenName { get; set; }
    }
}
