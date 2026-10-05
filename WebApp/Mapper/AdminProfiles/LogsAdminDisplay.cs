using AutoMapper;
using Common.DTOs.Log;
using WebApp.Models.Admin;

namespace WebApp.Mapper
{
    public class LogsAdminDisplay : AutoMapper.Profile
    {
        public LogsAdminDisplay() 
        {
            CreateMap<LogReadDTO, LogsAdminVM>();
        }
    }
}
