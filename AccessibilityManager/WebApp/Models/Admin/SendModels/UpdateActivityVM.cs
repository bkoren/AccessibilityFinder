namespace WebApp.Models.Admin.SendModels
{
    public class UpdateActivityVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Type { get; set; }
        public List<int>? AccessibilitiesId { get; set; } = new List<int>();
    }
}
