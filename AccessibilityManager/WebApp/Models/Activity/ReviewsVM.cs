namespace WebApp.Models.Activity
{
    public class ReviewsVM
    {
        public string Username { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Grade { get; set; }
    }
}
