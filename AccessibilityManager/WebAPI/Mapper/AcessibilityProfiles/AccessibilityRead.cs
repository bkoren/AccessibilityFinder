using AutoMapper;
using WebAPI.Models;
using Common.DTOs.Accessibility;

namespace WebAPI.Mapper
{
    public class AccessibilityRead : Profile
    {
        public AccessibilityRead()
        {
            CreateMap<Accessibility, AccessibilityReadDTO>();
        }
    }
}
