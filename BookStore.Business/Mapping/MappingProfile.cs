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

            CreateMap<Author, AuthorDto>()
                .ForMember(
                    d => d.FirstName,
                    o => o.MapFrom(s => s.User.FirstName)
                )
                .ForMember(
                    d => d.LastName,
                    o => o.MapFrom(s => s.User.LastName)
                )
                .ForMember(
                    d => d.UserId,
                    o => o.MapFrom(s => s.User.Id)
                );
            CreateMap<AuthorToAddDto, Author>();
            CreateMap<AuthorToUpdateDto, Author>();
        }
    }
}
