using AutoMapper;
using Common.DTOs.User;
using WebApp.Models.Admin;

namespace WebApp.Mapper
{
    public class UserAdminDisplay : AutoMapper.Profile
    {
        public UserAdminDisplay() 
        {
            CreateMap<UserReadDTO, UsersAdminVM>();
        }
    }
}
