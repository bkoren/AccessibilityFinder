using AutoMapper;
using WebAPI.Models;
using Common.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Context;
using WebAPI.Security;

namespace WebAPI.Controllers
{
    [Route("account")]
    [ApiController]
    public class Account : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly AccessibilityDbContext _context;
        public Account(AccessibilityDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                string username = userLoginDTO.Username.Trim();

                Models.User? user = _context.Users.FirstOrDefault(user => user.Username == username);
                if (user is null)
                    return StatusCode(401, 
                        new
                        {
                            target = "username",
                            message = "User doesn't exists!"
                        }
                    );

                if (PasswordHashProvider.GetHash(userLoginDTO.Password, user.PasswordSalt) != user.PasswordHash)
                    return StatusCode(401,
                        new
                        {
                            target = "password",
                            message = "Wrong password!"
                        }
                    );

                return Ok(
                    new 
                    { 
                        token = JwtTokenProvider.CreateToken(user.Username, user.Id, user.IsAdmin!, _configuration["Jwt:SecureKey"]!, 10)
                    }
                );
            }
            catch (Exception error)
            {
                AddLog("Error", $"Unknown error ocurred trying to log in user {userLoginDTO.Username}, Error: {error.GetType()}.").RunSynchronously();

                return StatusCode(500);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {            
            string username = userRegisterDTO.Username.Trim();

            if (_context.Users.FirstOrDefault(user => user.Username == userRegisterDTO.Username) is not null)
                return StatusCode(409,
                    new 
                    { 
                        target = "username",
                        message = "Username already exists!" 
                    }
                );

            if (_context.Users.FirstOrDefault(user => user.Email == userRegisterDTO.Email) is not null)
                return StatusCode(409,
                    new
                    {
                        target = "email",
                        message = "Email already exists!"
                    }
                );

            Models.User user = _mapper.Map<Models.User>(userRegisterDTO);
              
            user.PasswordSalt = PasswordHashProvider.GetSalt();                              
            user.PasswordHash = PasswordHashProvider.GetHash(userRegisterDTO.Password, user.PasswordSalt); 

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();

                await AddLog("Info", $"New user was created, username: {userRegisterDTO.Username}.");

                return Ok(new { message = "Successful registration!"} );
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unknown error ocurred trying to register user {userRegisterDTO.Username}, Error: {error.Message}");
              
                return StatusCode(500);
            }
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            int id;

            Models.User? user = new();
          
            try
            {
                id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                user = await _context.Users.FindAsync(id);

                if (PasswordHashProvider.GetHash(changePasswordDTO.CurrentPassword, user!.PasswordSalt) != user.PasswordHash)
                    return StatusCode(401,
                        new
                        {
                            target = "password",
                            message = "Wrong password!"
                        }
                    );

                user.PasswordSalt = PasswordHashProvider.GetSalt();
                user.PasswordHash = PasswordHashProvider.GetHash(changePasswordDTO.NewPassword, user.PasswordSalt);            

                await _context.SaveChangesAsync();

                return Ok(new { message = "Successful password change!" });
            }
            catch (Exception error)
            {
                await AddLog("Error", $"Unknown error ocurred trying to change password for user {user?.Username}, Error: {error.Message}");                

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
