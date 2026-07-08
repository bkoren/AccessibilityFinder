namespace WebApp.Models
{
    public class ProfileVM
    {
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public string Role { get; set; } = null!;
    }

}
