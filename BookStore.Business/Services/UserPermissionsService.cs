using AutoMapper;
using BookStore.Business.Contracts;
using BookStore.Business.Dto;
using BookStore.Data.Contracts;
using BookStore.Domain;
using BookStore.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Business.Services
{
    public class UserPermissionsService : IUserPermissionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserPermissionsService> _logger;
        private readonly IMapper _mapper;

        public UserPermissionsService(
            IUnitOfWork unitOfWork,
            ILogger<UserPermissionsService> logger,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserPermissionsDto> GetPermissionsAsync(Guid userId)
        {
            var userInDb = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (userInDb == null)
            {
                var message = $"Can't find permissions for user with id {userId}.";
                _logger.LogWarning(message);

                throw new RecordNotFoundException(message);
            }

            var userPermissionsToReturn = _mapper.Map<User, UserPermissionsDto>(userInDb);
            _logger.LogInformation($"User with id {userId} and his permissions was got.");

            return userPermissionsToReturn;
        }

        public async Task<UserPermissionsDto> AddPermissionsAsync(Guid userId, IEnumerable<string> permissions)
        {
            var userInDb = await _unitOfWork.UserRepository.AddPermissionsAsync(userId, permissions);
            await _unitOfWork.SaveAsync();
            var userPermissionsToReturn = _mapper.Map<User, UserPermissionsDto>(userInDb);
            _logger.LogInformation($"Permissions for user with id {userId} was successfully added.");

            return userPermissionsToReturn;
        }

        public async Task<UserPermissionsDto> RemovePermissionsAsync(Guid userId, IEnumerable<string> permissions)
        {
            var userInDb = await _unitOfWork.UserRepository.RemovePermissionsAsync(userId, permissions);
            await _unitOfWork.SaveAsync();
            var userPermissionsToReturn = _mapper.Map<User, UserPermissionsDto>(userInDb);
            _logger.LogInformation($"Permissions for user with id {userId} was successfully removed.");

            return userPermissionsToReturn;
        }
    }
}
