using AutoMapper;
using Common.DTOs.Review;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class ReviewCreate : Profile
    {
        public ReviewCreate() 
        {
            CreateMap<ReviewCreateDTO, Review>();           
        }
    }
}
