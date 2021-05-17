using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Domain;
using BookStore.Domain.Commands;
using BookStore.Domain.Events;

namespace BookStore.Business.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            SetUserMapping();
            SetAuthorMapping();
            SetBookMapping();
        }

        private void SetUserMapping()
        {
            CreateMap<User, UserDto>();

            CreateMap<UserDto, User>();

            CreateMap<User, UserPermissionsDto>();
        }

        private void SetAuthorMapping()
        {
            CreateMap<Book, AuthorBookDto>()
                .ForMember(d => d.BookId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.BookName, o => o.MapFrom(s => s.Name));

            CreateMap<Author, AuthorDto>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.User.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.User.LastName))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
                .ForMember(d => d.AuthorBooks, o => o.MapFrom(s => s.Books));

            CreateMap<AuthorToAddDto, Author>();

            CreateMap<AuthorToUpdateDto, Author>();
        }

        private void SetBookMapping()
        {
            CreateMap<Author, BookAuthorDto>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.User.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.User.LastName))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id));

            CreateMap<BookToAddDto, Book>()
                .ForMember(d => d.Authors, o => o.Ignore());

            CreateMap<Book, BookDto>();

            CreateMap<OrderCreatedEvent, AckOrderReservedCommand>()
                .ForMember(d => d.MessageType, o => o.Ignore());
            CreateMap<AckOrderReservedCommand, OrderReservedEvent>()
                .ForMember(d => d.MessageType, o => o.Ignore());
        }
    }
}
