using AutoMapper;
using Common.DTOs.Type;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("type")]
    [ApiController]
    public class Type : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AccessibilityDbContext _context;

        public Type(AccessibilityDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<TypeReadDTO>>> Get()
        {
            List<Models.Type> types = await _context.Types.ToListAsync();

            return (types.Count > 0)
                ? Ok(_mapper.Map<List<TypeReadDTO>>(types))
                : NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<ActionResult> Add(TypeCreateDTO DTO)
        {
            Models.Type? type = await _context.Types.FirstOrDefaultAsync(t => t.Name == DTO.Name);

            if (type != null)
                return BadRequest("Type already exists!");

            try
            {
                type = _mapper.Map<Models.Type>(DTO);

                _context.Types.Add(type);

                await _context.SaveChangesAsync();

                await AddLog("Info", $"Type {DTO.Name}, successfully added.");

                return Ok(type.Id);
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unexpected error occurred while trying to create type {DTO.Name}, Error: {error.GetType()}");

                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Models.Type? type = await _context.Types.FindAsync(id);

            if (type == null)
                return BadRequest("Type doesn't exists!");

            Activity? activity = await _context.Activities.FirstOrDefaultAsync(a => a.TypeId == type.Id);

            if (activity != null)
                return BadRequest("Type is in use!");

            try
            {
                _context.Types.Remove(type);

                await _context.SaveChangesAsync();

                await AddLog("Info", $"Type {type.Name} successfully deleted!");

                return Ok();
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unexpected error occurred while trying to delete type with id={id}, Error: {error.GetType()}");

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
