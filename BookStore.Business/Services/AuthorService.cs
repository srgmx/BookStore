using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Data.Specifications;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;


namespace BookStore.Business.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<AuthorService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
        {
            var authorWithUserInfoSpec = new AuthorWithUserInfoSpecification();
            var authors = await _unitOfWork.AuthorRepository.GetAllAsync(authorWithUserInfoSpec);
            var authorsToReturn = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(authors);
            _logger.LogInformation("Authors are received: {Data}", JsonSerializer.Serialize(authorsToReturn));

            return authorsToReturn;
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(Guid id)
        {
            var authorWithUserInfoSpec = new AuthorWithUserInfoSpecification(id);
            var authorInDb = await _unitOfWork.AuthorRepository.GetAsync(authorWithUserInfoSpec);
            CheckAuthorExists(authorInDb);
            var authorToReturn = _mapper.Map<Author, AuthorDto>(authorInDb);
            _logger.LogInformation("Author is received: {Data}", JsonSerializer.Serialize(authorToReturn));

            return authorToReturn;
        }

        public async Task<AuthorDto> AddAuthorAsync(AuthorToAddDto author)
        {
            var authorByUserIdSpec = new AuthorByUserIdSpecification(author.UserId);
            var authorInDb = await _unitOfWork.AuthorRepository.GetAsync(authorByUserIdSpec);

            if (authorInDb != null)
            {
                string message = $"User with UserId={author.UserId} is an author already.";
                _logger.LogWarning(message);

                throw new ExistingAuthorException(message);
            }

            var authorToAdd = _mapper.Map<AuthorToAddDto, Author>(author);
            authorInDb = await _unitOfWork.AuthorRepository.AddAsync(authorToAdd);
            await _unitOfWork.SaveAsync();
            var authorToReturn = await GetAuthorByIdAsync(authorInDb.Id);
            _logger.LogInformation("Author is added: {Data}", JsonSerializer.Serialize(authorToReturn));

            return authorToReturn;
        }

        public async Task<AuthorDto> UpdateAuthorAsync(AuthorToUpdateDto author)
        {
            var authorInDb = await _unitOfWork.AuthorRepository.GetByIdAsync(author.Id);
            CheckAuthorExists(authorInDb);
            var authorToUpdate = _mapper.Map<AuthorToUpdateDto, Author>(author);
            authorInDb = await _unitOfWork.AuthorRepository.UpdateAsync(authorToUpdate);
            await _unitOfWork.SaveAsync();
            var authorToReturn = await GetAuthorByIdAsync(authorInDb.Id);
            _logger.LogInformation("Author is updated: {Data}", JsonSerializer.Serialize(authorToReturn));

            return authorToReturn;
        }

        public async Task<bool> RemoveAuthorAsync(Guid id)
        {
            var authorInDb = await _unitOfWork.AuthorRepository.GetByIdAsync(id);
            CheckAuthorExists(authorInDb);
            _unitOfWork.AuthorRepository.Remove(authorInDb);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation($"Author with id {id} is removed.");

            return true;
        }

        private void CheckAuthorExists(Author author)
        {
            if (author == null)
            {
                var message = "Author was not found.";
                _logger.LogWarning(message);

                throw new RecordNotFoundException(message);
            }
        }
    }
}
