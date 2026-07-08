using AutoMapper;
using Common.DTOs.Review;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class ReviewUpdate : Profile
    {
        public ReviewUpdate() 
        {
            CreateMap<ReviewUpdateDTO, Review>()
                .ForMember(review => review.UserId, option => option.Ignore())
                .ForMember(review => review.ActivityId, option => option.Ignore())
                .ForAllMembers(check => check.Condition((DTO, Review, ReviewMember) => ReviewMember != null));
        }
    }
}
