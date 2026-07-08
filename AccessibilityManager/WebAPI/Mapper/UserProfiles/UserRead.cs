using AutoMapper;
using Common.DTOs.User;
using WebAPI.Models;

namespace WebAPI.Mapper.UserProfiles
{
    public class UserRead : Profile
    {
        public UserRead() 
        {
            CreateMap<User, UserReadDTO>()
                .ForMember(DTO => DTO.Role, useUser => useUser.MapFrom(user => (user.IsAdmin == "true") ? "Admin" : "User"));
        }
    }
}
