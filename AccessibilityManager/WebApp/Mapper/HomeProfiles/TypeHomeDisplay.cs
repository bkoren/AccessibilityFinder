using AutoMapper;
using Common.DTOs.Type;
using WebApp.Models.Filters;

namespace WebApp.Mapper
{
    public class TypeDisplay : AutoMapper.Profile
    {
        public TypeDisplay() 
        {
            CreateMap<TypeReadDTO, TypeVM>();
        }
    }
}
