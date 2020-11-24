using BookStore.Domain;
using System;

namespace BookStore.Data.Specifications
{
    public class AuthorWithUserInfoSpecification : BaseSpecification<Author>
    {
        public AuthorWithUserInfoSpecification() : base()
        {
            ConfigureInclude();
        }

        public AuthorWithUserInfoSpecification(Guid authorId) :
            base(u => u.Id == authorId)
        {
            ConfigureInclude();
        }

        private void ConfigureInclude()
        {
            AddInclude(a => a.User);
        }
    }
}
