using AutoMapper;
using Common.DTOs.Account;
using WebAPI.Models;

namespace WebAPI.Mapper.UserProfiles
{
    public class UserRegistration : Profile
    {
        public UserRegistration() 
        {
            CreateMap<UserRegisterDTO, User>();
        }
    }
}
