using BookStore.Domain;
using System;

namespace BookStore.Data.Specifications
{
    public class BookWithAuthorsSpecification : BaseSpecification<Book>
    {
        public BookWithAuthorsSpecification(Guid id) :
            base(b => b.Id == id)
        {
            AddInclude($"{nameof(Book.Authors)}.{nameof(Author.User)}");
        }
    }
}
