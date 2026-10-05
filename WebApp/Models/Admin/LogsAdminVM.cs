namespace WebApp.Models.Admin
{
    public class LogsAdminVM
    {
        public int Id { get; set; }
        public string Level { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Timestamp { get; set; } = null!;
    }
}
