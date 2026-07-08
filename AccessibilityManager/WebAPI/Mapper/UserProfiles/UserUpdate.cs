using AutoMapper;
using Common.DTOs.User;
using WebAPI.Models;

namespace WebAPI.Mapper.UserProfiles
{
    public class UserUpdate : Profile
    {
        public UserUpdate() 
        {
            CreateMap<UserUpdateDTO, User>()
                .ForAllMembers(check => check.Condition((User, DTO, UserMember) => UserMember != null));
        }
    }
}
