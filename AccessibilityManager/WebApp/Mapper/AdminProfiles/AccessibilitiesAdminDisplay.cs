using AutoMapper;
using Common.DTOs.Accessibility;
using WebApp.Models.Admin;

namespace WebApp.Mapper
{
    public class AccessibilitiesAdminDisplay : AutoMapper.Profile
    {
        public AccessibilitiesAdminDisplay() 
        { 
            CreateMap<AccessibilityReadDTO, AccessibilityAdminVM>();
        }
    }
}
