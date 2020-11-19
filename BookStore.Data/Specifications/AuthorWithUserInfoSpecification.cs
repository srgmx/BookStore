using BookStore.Domain;
using System;

namespace BookStore.Data.Specifications
{
    public class AuthorWithUserInfoSpecification : BaseSpecification<Author>
    {
        public AuthorWithUserInfoSpecification(Guid authorId) :
            base(u => u.Id == authorId)
        {
            AddInclude(a => a.User);
        }
    }
}
