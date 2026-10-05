using AutoMapper;
using WebAPI.Models;
using Common.DTOs.Activity;

namespace WebAPI.Mapper
{
    public class ActivityUpdate : Profile
    {
        public ActivityUpdate() 
        {
            CreateMap<ActivityUpdateDTO, Activity>()
                .ForMember(activity => activity.ActivityAccessibilities, option => option.Ignore())
                .ForMember(activity => activity.Reviews, option => option.Ignore())
                .ForMember(activity => activity.Type, option => option.Ignore())
                .ForAllMembers(check => check.Condition((Activity, DTO, ActivityMember) => ActivityMember != null));
        }
    }
}
