using AutoMapper;
using Common.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("user")]
    [ApiController]
    public class User : ControllerBase
    {
        private readonly AccessibilityDbContext _context;
        private readonly IMapper _mapper;

        public User(AccessibilityDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<List<UserReadDTO>>> All()
        {
            List<Models.User> users = await _context.Users.ToListAsync();

            return (users.Count > 0)
                ? Ok(_mapper.Map<List<UserReadDTO>>(users))
                : NoContent();
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<ActionResult<UserReadDTO>> Get(int id)
        {
            Models.User? user = await _context.Users.FindAsync(id); 

            return (user is not null)
                ? Ok(_mapper.Map<UserReadDTO>(user))
                : NotFound();
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<UserUpdateDTO>> Update(UserUpdateDTO DTO)
        {
            int id;

            Models.User? user = new();

            try
            {
                id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                user = await _context.Users.FindAsync(id);
            
                _mapper.Map(DTO,  user);

                await _context.SaveChangesAsync();

                await AddLog("Info", $"User {DTO.Username} was updated!");

                return Ok(DTO);
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unexpected error occurred while trying to update user {DTO.Username}, Error: {error.GetType()}");

                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Models.User? user = await _context.Users.FindAsync(id);

            if (user is null)
                return BadRequest("User doesn't exists!");

            if(user.IsAdmin == "true")
                return BadRequest("Can't delete admin!");

            List<Review> reviews = await _context.Reviews
                .Where(r => r.UserId == id)
                .ToListAsync();

            try
            {
                if (reviews.Count > 0)
                {
                    _context.RemoveRange(reviews);

                    await _context.SaveChangesAsync();
                }

                _context.Users.Remove(user);
                
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception error)
            {
                _context.ChangeTracker.Clear();
                await AddLog("Error", $"Unexpected error occurred while trying to delete user with id={id}, Error: {error.GetType()}");

                return StatusCode(500);
            }           
        }

        private async Task AddLog(string level, string messeage)
        {
            _context.Logs.Add(new Log
            {
                Level = level,
                Message = messeage
            });
            await _context.SaveChangesAsync();
        }
    }
}
