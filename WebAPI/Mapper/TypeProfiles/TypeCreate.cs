using AutoMapper;
using Common.DTOs.Type;

using Type = WebAPI.Models.Type;

namespace WebAPI.Mapper.TypeProfiles
{
    public class TypeCreate : Profile
    {
        public TypeCreate() 
        {
            CreateMap<TypeCreateDTO, Type>();
        }
    }
}
