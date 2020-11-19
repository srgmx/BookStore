using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Data.Specifications;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(
            IGenericRepository<Author> authorRepository, 
            IMapper mapper
        )
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDto> AddAuthorAsync(AuthorToAddDto author)
        {
            var authorByUserIdSpec = new AuthorByUserIdSpecification(author.UserId);
            var authorInDb = await _authorRepository.FindAsync(authorByUserIdSpec);

            if (authorInDb != null)
            {
                string message = $"User with UserId={author.UserId} is an author already.";

                throw new ExistingAuthorException(message);
            }

            var authorToAdd = _mapper.Map<AuthorToAddDto, Author>(author);
            authorInDb = await _authorRepository.AddAsync(authorToAdd);
            var authorToReturn = await GetAuthorByIdAsync(authorInDb.Id);

            return authorToReturn;
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(Guid id)
        {
            var authorWithUserInfoSpec = new AuthorWithUserInfoSpecification(id);
            var authorInDb = await _authorRepository.FindAsync(authorWithUserInfoSpec);
            var authorToReturn = _mapper.Map<Author, AuthorDto>(authorInDb);

            return authorToReturn;
        }
    }
}
