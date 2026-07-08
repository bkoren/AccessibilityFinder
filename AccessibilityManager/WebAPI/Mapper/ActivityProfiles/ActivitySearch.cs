using AutoMapper;
using Common.DTOs.Activity;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class ActivitySearch : Profile
    {
        public ActivitySearch() 
        {
            CreateMap<Activity, ActivitySearchDTO>()
                .ForMember(DTO => DTO.Type, useActivity => useActivity.MapFrom(activity => activity.Type!.Name));
        }
    }
}
