using AutoMapper;
using Common.DTOs.User;
using WebApp.Models;

namespace WebApp.Mapper
{
    public class UserProfileDisplay : AutoMapper.Profile
    {
        public UserProfileDisplay() 
        {
            CreateMap<UserReadDTO, ProfileVM>();
        }
    }
}
