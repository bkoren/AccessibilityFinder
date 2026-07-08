using AutoMapper;
using Common.DTOs.Accessibility;
using WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;

namespace WebAPI.Controllers
{
    [Route("accessibility")]
    [ApiController]
    public class Accessibility : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly AccessibilityDbContext _context;

        public Accessibility(AccessibilityDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<AccessibilityReadDTO>>> All()
        {
            List<Models.Accessibility> accessibility = await _context.Accessibilities                
                .ToListAsync();

            return (accessibility.Count > 0)
                ? Ok(_mapper.Map<List<AccessibilityReadDTO>>(accessibility))
                : NoContent();
          
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<ActionResult> Add([FromBody] AccessibilityCreateDTO accessibilityCreateDTO)
        {
            if (_context.Accessibilities.Any(accessibilities => accessibilities.Name == accessibilityCreateDTO.Name))
                return BadRequest("Accessivility already exists!");                      

            try
            {
                Models.Accessibility accessibility = _mapper.Map<Models.Accessibility>(accessibilityCreateDTO);

                _context.Accessibilities.Add(accessibility);           
              
                await _context.SaveChangesAsync();
                await AddLog("Info", $"Accessibility \"{accessibilityCreateDTO.Name}\" created");

                return Ok( new { accessibility.Id });
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unexpected error occurred while trying to create accessibility \"{accessibilityCreateDTO.Name}\", Error: {error.GetType()}");

                return StatusCode(500);                
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Models.Accessibility? accessibility = await _context.Accessibilities.FindAsync(id);

            if (accessibility is null)
                return NotFound();
          
            try
            {
                List<ActivityAccessibility>? activityAccessibility = await _context.ActivityAccessibilities
                    .Where(a => a.AccessibilityId == id)
                    .ToListAsync();
              
                _context.ActivityAccessibilities.RemoveRange(activityAccessibility);                                                  
                _context.Accessibilities.Remove(accessibility);
                
                await _context.SaveChangesAsync();

                await AddLog("Info", $"Accessibility with id={id} deleted");

                return Ok();
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unexpected error occurred while trying to delete accessibility with id={id}, Error: {error.GetType()}");

                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<AccessibilityUpdateDTO>> Update(int id, [FromBody] AccessibilityUpdateDTO accessibilityUpdateDTO)
        {
            Models.Accessibility? accessibility = await _context.Accessibilities.FindAsync(id);

            if(accessibility is null)
                return NotFound();

            try
            {
                _mapper.Map(accessibilityUpdateDTO, accessibility);

                await _context.SaveChangesAsync();
                await AddLog("Info", $"Accessibility with id={id} updated");

                return Ok(accessibilityUpdateDTO);
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unexpected error occurred while updating accessibility with id={id}, Error: {error.GetType()}");

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
