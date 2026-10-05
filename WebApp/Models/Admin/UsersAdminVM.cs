namespace WebApp.Models.Admin
{
    public class UsersAdminVM
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
