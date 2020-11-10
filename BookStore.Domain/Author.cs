using System.Collections.Generic;

namespace BookStore.Domain
{
    public class Author
    {
        public Author()
        {
            this.Books = new List<Book>();
        }

        public int Id { get; set; }

        public string PenName { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

        public List<Book> Books { get; set; }
    }
}
