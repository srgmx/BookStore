using System.Threading.Tasks;

namespace BookStore.Data.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IAuthorRepository AuthorRepository { get; }

        IBookRepopository BookRepository { get; }

        Task SaveAsync(); 
    }
}
