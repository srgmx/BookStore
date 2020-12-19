using System.Collections.Generic;

namespace BookStore.Domain
{
    public class User : BaseEntity
    {
        public User()
        {
            Permissions = new List<string>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Author Author { get; set; }

        public List<string> Permissions { get; set; }
    }
}
