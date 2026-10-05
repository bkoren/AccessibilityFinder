using AutoMapper;
using Common.DTOs.Activity;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class ActivityGet : Profile
    {
        public ActivityGet()
        {
            CreateMap<Activity, ActivityGetDTO>()
            .ForMember(DTO => DTO.Type, useActivity => useActivity.MapFrom(activity => activity.Type))
            .ForMember(DTO => DTO.Accessibilities,
                 useActivity => useActivity.MapFrom(activity => activity.ActivityAccessibilities
                    .Select(activityAccessibilities => activityAccessibilities.Accessibility)
                    .ToList()
                 )
            );
        }
    }
}
