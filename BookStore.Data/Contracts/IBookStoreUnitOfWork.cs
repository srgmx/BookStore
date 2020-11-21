using System.Threading.Tasks;

namespace BookStore.Data.Contracts
{
    public interface IBookStoreUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IAuthorRepository AuthorRepository { get; }

        Task SaveAsync(); 
    }
}
