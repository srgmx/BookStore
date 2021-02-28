using BookCoreLibrary.Domain.Models;
using System;
using System.Collections.Generic;

namespace BookStore.Domain
{
    public class Author : BaseEntity
    {
        public Author()
        {
            Books = new List<Book>();
        }

        public string PenName { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public List<Book> Books { get; set; }
    }
}
