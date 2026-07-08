using AutoMapper;
using WebAPI.Models;
using Common.DTOs.Activity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebAPI.Context;

namespace WebAPI.Controllers
{
    [Route("activity")]
    [ApiController]
    public class Activities : ControllerBase
    {
        private readonly AccessibilityDbContext _context;
        private readonly IMapper _mapper;

        public Activities(AccessibilityDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("count")]
        public ActionResult Count()
        {
            return Ok(_context.Activities.Count());
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ActivityGetDTO>>> All()
        {
            List<Activity> activities = await _context.Activities
                .Include(a => a.ActivityAccessibilities)
                    .ThenInclude(aa => aa.Accessibility)
                .Include(a => a.Reviews)
                    .ThenInclude(r => r.User)
                .Include(a => a.Type)
                .ToListAsync();

            return (activities.Count > 0)
                ? Ok(_mapper.Map<List<ActivityGetDTO>>(activities))
                : NoContent();
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ActivityGetDTO>> Get(int id)
        {
            Activity? activity = await _context.Activities
                .Include(a => a.ActivityAccessibilities).ThenInclude(aa => aa.Accessibility)                    
                .Include(a => a.Reviews).ThenInclude(r => r.User)
                .Include(a => a.Type)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity != null)
            {
                activity.Views++;

                await _context.SaveChangesAsync();
            }

            return (activity != null)
                ? Ok(_mapper.Map<ActivityGetDTO>(activity))
                : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<ActionResult> Add([FromBody] ActivityCreateDTO DTO)
        {
            if (_context.Activities.Any(a => a.Name == DTO.Name))
                return BadRequest("Activity aready exists!");

            Models.Type? type = await _context.Types.FirstOrDefaultAsync(t => t.Name == DTO.Type);

            if (type is null)
                return BadRequest($"Type \"{DTO.Type}\" not supported!");

            Activity activity = new();
            try
            {
                List<Models.Accessibility> accessibilities = await _context.Accessibilities
                    .Where(a => DTO.Accessibilities.Contains(a.Name))
                    .ToListAsync();

                if (accessibilities.Count != DTO.Accessibilities.Count)
                {
                    List<string> notFoundAccessibilities = DTO.Accessibilities
                        .Where(a => accessibilities.All(aa => aa.Name != a))
                        .ToList();
                    return BadRequest($"Accessibilities \"{string.Join(", ", notFoundAccessibilities)}\" not supported!");
                }

                activity = _mapper.Map<Activity>(DTO);
                activity.TypeId = type?.Id;
                activity.Type = type;
                activity.Views = 0;

                _context.Activities.Add(activity);

                await _context.SaveChangesAsync();

                List<ActivityAccessibility> activityAccessibilities = accessibilities
                    .Select(a => new ActivityAccessibility
                    {
                        ActivityId = activity.Id,
                        AccessibilityId = a.Id
                    })
                    .ToList();

                _context.ActivityAccessibilities.AddRange(activityAccessibilities);
              
                await _context.SaveChangesAsync();                                    
                await AddLog("Info", $"Activity \"{activity.Name}\" created");

                return Ok(new { activity.Id });
            }
            catch (Exception error)
            {
                _context.ChangeTracker.Clear();                
                await AddLog("Error", $"Unexpected error occurred while trying to create activity \"{activity.Name}\", Error: {error.GetType().ToString() ?? "Unknown erro!"}");

                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<ActivityUpdateDTO>> Update(int id, [FromBody] ActivityUpdateDTO DTO)
        {
            Activity? activity = await _context.Activities
                .Include(a => a.Type)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity is null)
                return NotFound();

            if (_context.Activities.Any(a => a.Name == DTO.Name))
                return BadRequest("Can't have activity with duplicate name!");

            Models.Type? type = await _context.Types.FirstOrDefaultAsync(t => t.Name == DTO.Type);

            if (type != null)            
                activity.Type = type;
            
            else if(type is null && !string.IsNullOrEmpty(DTO.Type))
                return BadRequest($"Type '{DTO.Type}' not supported.");

            _mapper.Map(DTO, activity);

            if (DTO.Accessibilities?.Count > 0)
            {
                List<Models.Accessibility> accessibilities = await _context.Accessibilities
                    .Where(a => DTO.Accessibilities.Contains(a.Name))
                    .ToListAsync();

                List<ActivityAccessibility> activityAccessibilities = await _context.ActivityAccessibilities
                    .Where(aa => aa.ActivityId == activity.Id)
                    .ToListAsync();

                List<ActivityAccessibility> result = new();
                foreach (Models.Accessibility accessibility in accessibilities)
                {
                    if(activityAccessibilities.All(aa => aa.AccessibilityId != accessibility.Id))
                    {
                        result.Add(new ActivityAccessibility
                        {
                            Activity = activity,
                            Accessibility = accessibility
                        });
                    }
                }

                await _context.ActivityAccessibilities.AddRangeAsync(result);
            }

            try
            {                                     
                await _context.SaveChangesAsync();
                await AddLog("Info", $"Activity \"{activity.Name}\" with id={id} updated");
              
                return Ok(DTO);
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unexpected error occurred while updating activity with id={activity.Id}, Error: {error.GetType()}");

                return StatusCode(500);                
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Activity? activity = await _context.Activities.FirstOrDefaultAsync(activity => activity.Id == id);

            if (activity is null)
                return NotFound();

            List<Review>? reviews = await _context.Reviews.Where(review => review.ActivityId == activity.Id).ToListAsync();
            List<ActivityAccessibility>? activityAccessibilities = await _context.ActivityAccessibilities
                .Where(aa => aa.ActivityId == activity.Id)
                .ToListAsync();

            try
            {
                _context.Reviews.RemoveRange(reviews);
                _context.ActivityAccessibilities.RemoveRange(activityAccessibilities);

                await _context.SaveChangesAsync();                
              
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();                
                await AddLog("Info", $"Activity \"{activity.Name}\" with id={id} deleted");

                return Ok();
            }
            catch (Exception error)
            {
                _context.ChangeTracker.Clear();
                await AddLog("Error", $"Unexpected error occurred while trying to delete activity with id={id}, Error: {error.GetType()}");                

                return StatusCode(500);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ActivitySearchDTO>>> Search(string? name = null, int page = 1, int typeId = 0, [FromQuery] List<int>? accessibilityId = null)
        {
            IQueryable<Activity> query = _context.Activities
                .Include(a => a.Type)
                .Include(a => a.ActivityAccessibilities)
                    .ThenInclude(aa => aa.Accessibility)
                    .AsQueryable();
        
            if (!string.IsNullOrEmpty(name))
                query = query.Where(a => a.Name.Contains(name));

            if(typeId != 0)
                query = query.Where(a => a.TypeId == typeId);

            if(accessibilityId != null && accessibilityId.Count > 0)
                query = query.Where(a =>
                    a.ActivityAccessibilities
                        .Any(aa => accessibilityId.Contains(aa.AccessibilityId))
                );

            const int count = 4;
            List<Activity> activities = await query
                .Skip((page - 1) * count) 
                .Take(count)
                .ToListAsync();                

            return _mapper.Map<List<ActivitySearchDTO>>(activities);
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