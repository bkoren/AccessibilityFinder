using AutoMapper;
using Common.DTOs.Accessibility;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class AccessibilityCreate : Profile
    {
        public AccessibilityCreate() 
        {
            CreateMap<AccessibilityCreateDTO, Accessibility>();
        }
    }
}
