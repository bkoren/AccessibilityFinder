using AutoMapper;
using WebApp.Models.Filters;
using Common.DTOs.Accessibility;

namespace WebApp.Mapper
{
    public class AccessibilityHomeDisplay : AutoMapper.Profile
    {
        public AccessibilityHomeDisplay()
        {
            CreateMap<AccessibilityReadDTO, AccessibilitiesVM>();
        }
    }
}
