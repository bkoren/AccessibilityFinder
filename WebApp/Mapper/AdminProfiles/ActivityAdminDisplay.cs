using AutoMapper;
using Common.DTOs.Activity;
using WebApp.Models.Admin;

namespace WebApp.Mapper
{
    public class ActivityAdminDisplay : AutoMapper.Profile
    {
        public ActivityAdminDisplay() 
        {
            CreateMap<ActivityGetDTO, ActivityAdminVM>()
                .ForMember(VM => VM.Type, useDTO => useDTO.MapFrom(DTO => DTO.Type.Name));
        }
    }
}
