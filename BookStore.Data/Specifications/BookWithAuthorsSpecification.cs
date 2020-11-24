using BookStore.Domain;
using System;

namespace BookStore.Data.Specifications
{
    public class BookWithAuthorsSpecification : BaseSpecification<Book>
    {
        public BookWithAuthorsSpecification() :
            base()
        {
            ConfigureInclude();
        }

        public BookWithAuthorsSpecification(Guid id) :
            base(b => b.Id == id)
        {
            ConfigureInclude();
        }

        private void ConfigureInclude()
        {
            AddInclude($"{nameof(Book.Authors)}.{nameof(Author.User)}");
        }
    }
}
