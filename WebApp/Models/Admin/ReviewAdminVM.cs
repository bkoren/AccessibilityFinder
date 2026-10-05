namespace WebApp.Models.Admin
{
    public class ReviewAdminVM
    {
        public int Id { get; set; }
        public int Grade { get; set; }      
        public string Comment { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Activity { get; set; } = null!;
    }
}
