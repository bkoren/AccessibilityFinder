
namespace WebAPI.Models;

public partial class Activity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Address { get; set; }

    public string? Contact { get; set; }

    public string? Email { get; set; }

    public int? TypeId { get; set; }

    public int? Views { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = [];

    public virtual Type? Type { get; set; }

    public ICollection<ActivityAccessibility> ActivityAccessibilities { get; set; } = new List<ActivityAccessibility>();
}
