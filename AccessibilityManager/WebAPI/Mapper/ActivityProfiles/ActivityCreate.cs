using AutoMapper;
using Common.DTOs.Activity;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class ActivityCreate : Profile
    {
        public ActivityCreate()
        {
            CreateMap<ActivityCreateDTO, Activity>()
             .ForMember(activity => activity.ActivityAccessibilities, option => option.Ignore())
             .ForMember(activity => activity.Reviews, option => option.Ignore())
             .ForMember(activity => activity.Type, option => option.Ignore());
        }
    }
}
