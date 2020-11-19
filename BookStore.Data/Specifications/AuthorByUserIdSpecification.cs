using BookStore.Domain;
using System;

namespace BookStore.Data.Specifications
{
    public class AuthorByUserIdSpecification : BaseSpecification<Author>
    {
        public AuthorByUserIdSpecification(Guid userId) :
            base(a => a.UserId == userId)
        {
            AddInclude(a => a.User);
        }
    }
}
