namespace BookStore.Business.Dto
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PenName { get; set; }
    }
}