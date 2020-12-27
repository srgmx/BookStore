using BookStore.Domain;
using System;

namespace BookStore.Data.MSSQL.Specifications
{
    public class AuthorWithUserAndBooksSpecification : BaseSpecification<Author>
    {
        public AuthorWithUserAndBooksSpecification() : base()
        {
            ConfigureInclude();
        }

        public AuthorWithUserAndBooksSpecification(Guid authorId) :
            base(u => u.Id == authorId)
        {
            ConfigureInclude();
        }

        private void ConfigureInclude()
        {
            AddInclude(a => a.User);
            AddInclude(a => a.Books);
        }
    }
}
