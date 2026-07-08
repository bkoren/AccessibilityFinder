using AutoMapper;
using Common.DTOs.Review;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class ReviewRead : Profile
    {
        public ReviewRead() 
        {
            CreateMap<Review, ReviewReadDTO>()
                .ForMember(DTO => DTO.Username, useReview => useReview.MapFrom(review => review.User.Username))
                .ForMember(DTO => DTO.Activity, useReview => useReview.MapFrom(review => review.Activity.Name))
                .ForMember(DTO => DTO.Date, useReview => useReview.MapFrom(review => review.Date.ToString()));
        }
    }
}
