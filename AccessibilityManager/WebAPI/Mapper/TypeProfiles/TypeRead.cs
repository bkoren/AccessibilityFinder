using AutoMapper;
using Common.DTOs.Type;

using Type = WebAPI.Models.Type;

namespace WebAPI.Mapper
{
    public class TypeRead : Profile
    {
        public TypeRead() 
        {
            CreateMap<Type, TypeReadDTO>();
        }
    }
}
