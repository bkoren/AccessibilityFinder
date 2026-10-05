using Common.DTOs;
using Common.DTOs.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
   [Route("log")]
   [ApiController]
    public class Logs : ControllerBase
    {
        private readonly AccessibilityDbContext _context;

        public Logs(AccessibilityDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("count")]
        public ActionResult Count()
        {
            return Ok(_context.Logs.Count());        
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<List<LogReadDTO>>> All()
        {
            List<Log> logs = await _context.Logs
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();
        
            return (logs.Count > 0)
                ? Ok(logs)
                : NoContent();
        }

        [Authorize]
        [HttpGet("get/{count}")]
        public async Task<ActionResult<List<LogReadDTO>>> GetLast(int count)
        {
            List<Log> logs = await _context.Logs
                .OrderByDescending(log => log.Timestamp)
                .Take(count)
                .ToListAsync();

            return Ok(logs);
        }
    }
}
