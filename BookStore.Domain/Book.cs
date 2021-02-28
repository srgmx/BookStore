using BookCoreLibrary.Domain.Models;
using System.Collections.Generic;

namespace BookStore.Domain
{
    public class Book : BaseEntity
    {
        public Book()
        {
            Authors = new List<Author>();
        }

        public string Name { get; set; }

        public List<Author> Authors { get; set; }
    }
}
