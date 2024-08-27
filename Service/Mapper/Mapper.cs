using AutoMapper;
using Repository.Entity;
using Repository.Request;
using Repository.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Book, BookResponse>().ReverseMap();
            CreateMap<CreateBookRequest, Book>().ReverseMap();

            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.RoleName, src => src.MapFrom(x => x.Role.Name));
            CreateMap<CreateUserRequest, User>();

            CreateMap<Book, RentingBookResponse>().ReverseMap();
        }
    }
}
