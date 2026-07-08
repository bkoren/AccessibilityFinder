namespace WebApp.Models.Admin
{
    public class ActivityAdminVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Views { get; set; }
    }
}
