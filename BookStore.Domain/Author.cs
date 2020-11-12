using System.Collections.Generic;

namespace BookStore.Domain
{
    public class Author: BaseEntity
    {
        public Author()
        {
            this.Books = new List<Book>();
        }


        public string PenName { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

        public List<Book> Books { get; set; }
    }
}
