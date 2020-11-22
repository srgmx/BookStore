using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IBookStoreUnitOfWork _bookStoreUnitOfWork;
        private readonly IMapper _mapper;

        public UserService(
            IBookStoreUnitOfWork bookStoreUnitOfWork,
            IMapper mapper
        )
        {
            _bookStoreUnitOfWork = bookStoreUnitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> AddUserAsync(UserDto user)
        {
            var userToAdd = _mapper.Map<UserDto, User>(user);
            var userInDb = await _bookStoreUnitOfWork.UserRepository.AddAsync(userToAdd);
            await _bookStoreUnitOfWork.SaveAsync();
            user.Id = userInDb.Id;

            return user;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var userInDb = await _bookStoreUnitOfWork.UserRepository.GetByIdAsync(id);
            CheckUserExists(userInDb);

            return _mapper.Map<User, UserDto>(userInDb);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _bookStoreUnitOfWork.UserRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }

        public async Task<bool> RemoveUserByIdAsync(Guid id)
        {
            var userInDb = await _bookStoreUnitOfWork.UserRepository.GetByIdAsync(id);
            CheckUserExists(userInDb);
            _bookStoreUnitOfWork.UserRepository.RemoveAsync(userInDb);
            await _bookStoreUnitOfWork.SaveAsync();

            return true;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto user)
        {
            var userInDb = await _bookStoreUnitOfWork.UserRepository.GetByIdAsync(user.Id);
            CheckUserExists(userInDb);
            var userToUpdate = _mapper.Map<UserDto, User>(user);
            userInDb = await _bookStoreUnitOfWork.UserRepository.UpdateAsync(userToUpdate);
            await _bookStoreUnitOfWork.SaveAsync();
            var userToReturn = _mapper.Map<User, UserDto>(userInDb);

            return userToReturn; 
        }

        private void CheckUserExists(User user)
        {
            if (user == null)
            {
                throw new RecordNotFoundException();
            }
        }
    }
}
