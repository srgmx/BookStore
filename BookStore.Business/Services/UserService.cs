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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> AddUserAsync(UserDto user)
        {
            var userToAdd = _mapper.Map<UserDto, User>(user);
            var userInDb = await _unitOfWork.UserRepository.AddAsync(userToAdd);
            await _unitOfWork.SaveAsync();
            user.Id = userInDb.Id;

            return user;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var userInDb = await _unitOfWork.UserRepository.GetByIdAsync(id);
            CheckUserExists(userInDb);

            return _mapper.Map<User, UserDto>(userInDb);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }

        public async Task<bool> RemoveUserByIdAsync(Guid id)
        {
            var userInDb = await _unitOfWork.UserRepository.GetByIdAsync(id);
            CheckUserExists(userInDb);
            _unitOfWork.UserRepository.Remove(userInDb);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto user)
        {
            var userInDb = await _unitOfWork.UserRepository.GetByIdAsync(user.Id);
            CheckUserExists(userInDb);
            var userToUpdate = _mapper.Map<UserDto, User>(user);
            userInDb = await _unitOfWork.UserRepository.UpdateAsync(userToUpdate);
            await _unitOfWork.SaveAsync();
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
