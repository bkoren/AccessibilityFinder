
namespace WebAPI.Models;

public partial class Accessibility
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<ActivityAccessibility> ActivityAccessibilities { get; set; } = new List<ActivityAccessibility>();
}
