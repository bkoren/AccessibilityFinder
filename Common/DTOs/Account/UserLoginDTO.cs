using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.Account;

public class UserLoginDTO
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

