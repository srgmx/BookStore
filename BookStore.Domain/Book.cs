using System.Collections.Generic;

namespace BookStore.Domain
{
    public class Book: BaseEntity
    {
        public Book()
        {
            this.Authors = new List<Author>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Author> Authors { get; set; }
    }
}
