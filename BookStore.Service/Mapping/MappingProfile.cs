using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Domain;

namespace BookStore.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
