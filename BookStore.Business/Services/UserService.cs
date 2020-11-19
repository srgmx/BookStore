using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IGenericRepository<User> userRepository,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> AddUserAsync(UserDto user)
        {
            var userToAdd = _mapper.Map<UserDto, User>(user);
            var userInDb = await _userRepository.AddAsync(userToAdd);
            user.Id = userInDb.Id;

            return user;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var userInDb = await _userRepository.FindByIdAsync(id);

            return _mapper.Map<User, UserDto>(userInDb);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> RemoveUserByIdAsync(Guid id)
        {
            var userInDb = await _userRepository.FindByIdAsync(id);

            if (userInDb == null)
            {
                return null;
            }

            var userRemoved = await _userRepository.RemoveAsync(userInDb);

            return _mapper.Map<User, UserDto>(userRemoved);
        }

        public async Task<UserDto> UpdateUserAsync(UserDto user)
        {
            var userInDb = await _userRepository.FindByIdAsync(user.Id);

            if (userInDb == null)
            {
                return null;
            }

            var userToUpdate = _mapper.Map<UserDto, User>(user);
            await _userRepository.UpdateAsync(userToUpdate);
            
            return user; 
        }
    }
}
