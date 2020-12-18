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

            CreateMap<Book, AuthorBookDto>()
                .ForMember(
                    d => d.BookId,
                    o => o.MapFrom(s => s.Id)
                )
                .ForMember(
                    d => d.BookName,
                    o => o.MapFrom(s => s.Name)
                );

            CreateMap<Author, BookAuthorDto>()
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
                )
                .ForMember(
                    d => d.AuthorBooks,
                    o => o.MapFrom(s => s.Books)
                );
            CreateMap<AuthorToAddDto, Author>();
            CreateMap<AuthorToUpdateDto, Author>();

            CreateMap<BookToAddDto, Book>()
                .ForMember(
                    d => d.Authors, 
                    o => o.Ignore()
                 );
            CreateMap<Book, BookDto>();
        }
    }
}
