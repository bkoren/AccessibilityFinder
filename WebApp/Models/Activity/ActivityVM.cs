namespace WebApp.Models.Activity
{
    public class ActivityVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TypeVM Type { get; set; }
        public string Contact { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public List<AccessibilitiesVM> Accessibilities { get; set; } = [];
        public List<ReviewsVM> Reviews { get; set; } = [];
    }
}
