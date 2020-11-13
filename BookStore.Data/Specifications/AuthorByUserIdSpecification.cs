using BookStore.Domain;


namespace BookStore.Data.Specifications
{
    public class AuthorByUserIdSpecification : BaseSpecification<Author>
    {
        public AuthorByUserIdSpecification(int userId)
            :base(a => a.UserId == userId)
        {
            this.AddInclude(a => a.User);
        }
    }
}
