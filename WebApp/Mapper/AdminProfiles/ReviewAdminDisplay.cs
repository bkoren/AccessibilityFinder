using AutoMapper;
using Common.DTOs.Review;
using WebApp.Models.Admin;

namespace WebApp.Mapper.AdminProfiles
{
    public class ReviewAdminDisplay : AutoMapper.Profile
    {
        public ReviewAdminDisplay() 
        {
            CreateMap<ReviewReadDTO, ReviewAdminVM>();
        }
    }
}
