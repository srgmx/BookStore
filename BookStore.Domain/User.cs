namespace BookStore.Domain
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Author Author  { get; set; }
    }
}
