using AutoMapper;
using Common.DTOs.Review;
using WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.Context;

namespace WebAPI.Controllers
{
    [Route("review")]
    [ApiController]
    public class Reviews : ControllerBase
    {
        private readonly AccessibilityDbContext _context;

        private readonly IMapper _mapper;
        public Reviews(AccessibilityDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<List<ReviewReadDTO>>> All()
        {
            List<Review>? reviews = await _context.Reviews
                .Include(review => review.User)
                .Include(review => review.Activity)
                .ToListAsync();

            return (reviews.Count > 0)
                ? Ok(_mapper.Map<List<ReviewReadDTO>>(reviews))
                : NoContent();
        }


        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ReviewReadDTO>> Get(int id)
        {
            Review? review = await _context.Reviews
                .Include(review => review.User)
                .Include(review => review.Activity)
                .FirstOrDefaultAsync(review => review.Id == id);

            return (review is not null)
                ? Ok(_mapper.Map<ReviewReadDTO>(review))
                : NotFound(); 
        }

        [Authorize]
        [HttpPut("update/{id}")] 
        public async Task<ActionResult> Update(int id, [FromBody] ReviewUpdateDTO reviewUpdateDTO)
        {
            Review? review = await _context.Reviews.FindAsync(id);

            if(review is null) 
                return NotFound();
            
            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (review.UserId != UserId)
                return Unauthorized();

            _mapper.Map(reviewUpdateDTO, review);

            try
            {
                await _context.SaveChangesAsync();
              
                return Ok(reviewUpdateDTO);
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unknown error ocurred while trying to update review with id={id}, Error: {error.GetType()}");

                return StatusCode(500);
            }
        }       

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult> Add([FromBody] ReviewCreateDTO reviewCreateDTO)
        {           
            int id;

            Models.User? user = new();

            try
            {
                id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                user = await _context.Users.FindAsync(id);                    

                Activity? activity = await _context.Activities.FirstOrDefaultAsync
                    (activity => activity.Id == reviewCreateDTO.ActivityId);
          
                if(activity is null)
                    return NotFound();

                Review review = _mapper.Map<Review>(reviewCreateDTO);

                review.UserId = user!.Id;
                review.ActivityId = activity.Id;

                _context.Reviews.Add(review);

                await _context.SaveChangesAsync();

                return Ok(reviewCreateDTO);
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unknown error ocurred while trying to create review for activity id=\"{reviewCreateDTO.ActivityId}\", Error: {error.GetType()}");

                return StatusCode(500);
            }                       
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Review? review = await _context.Reviews.FindAsync(id);

            if (review is null)
                return NotFound();

            _context.Reviews.Remove(review);

            try
            {
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unknown error ocurred while trying to delete review with id={id}, Error: {error.GetType()}");

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
