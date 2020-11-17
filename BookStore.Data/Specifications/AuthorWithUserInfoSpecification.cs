using BookStore.Domain;

namespace BookStore.Data.Specifications
{
    public class AuthorWithUserInfoSpecification : BaseSpecification<Author>
    {
        public AuthorWithUserInfoSpecification(int authorId) :
            base(u => u.Id == authorId)
        {
            AddInclude(a => a.User);
        }
    }
}
