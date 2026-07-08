using AutoMapper;
using Common.DTOs.Type;
using WebApp.Models.Admin;

namespace WebApp.Mapper
{
    public class TypeAdminDisplay : AutoMapper.Profile
    {
        public TypeAdminDisplay() 
        {
            CreateMap<TypeReadDTO, TypeAdminVM>();
        }
    }
}
