using AutoMapper;
using Common.DTOs.Accessibility;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class AccessibilityUpdate : Profile
    {
        public AccessibilityUpdate() 
        {
            CreateMap<AccessibilityUpdateDTO, Accessibility>();
        }
    }
}
