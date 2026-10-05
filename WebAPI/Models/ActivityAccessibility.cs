
namespace WebAPI.Models;

public partial class ActivityAccessibility
{
    public int ActivityId { get; set; }
    public int AccessibilityId { get; set; }

    public Activity Activity { get; set; } = null!;
    public Accessibility Accessibility { get; set; } = null!;
}

