using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> AddUserAsync(UserDto user)
        {
            var userToAdd = _mapper.Map<UserDto, User>(user);
            var userInDb = await _unitOfWork.UserRepository.AddAsync(userToAdd);
            await _unitOfWork.SaveAsync();
            user.Id = userInDb.Id;
            _logger.LogInformation("User is added: {Data}", JsonSerializer.Serialize(user));

            return user;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var userInDb = await _unitOfWork.UserRepository.GetByIdAsync(id);
            CheckUserExists(userInDb);
            var userToReturn = _mapper.Map<User, UserDto>(userInDb);
            _logger.LogInformation("User is received: {Data}", JsonSerializer.Serialize(userToReturn));

            return userToReturn;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var usersToReturn = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            _logger.LogInformation("Users are received: {Data}", JsonSerializer.Serialize(usersToReturn));

            return usersToReturn;
        }

        public async Task<bool> RemoveUserByIdAsync(Guid id)
        {
            await _unitOfWork.UserRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation($"Users with id {id} is removed.");

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
            _logger.LogInformation("Users is updated: {Data}", JsonSerializer.Serialize(userToReturn));

            return userToReturn; 
        }

        private void CheckUserExists(User user)
        {
            if (user == null)
            {
                var message = "User was not found.";
                _logger.LogWarning(message);

                throw new RecordNotFoundException(message);
            }
        }
    }
}
