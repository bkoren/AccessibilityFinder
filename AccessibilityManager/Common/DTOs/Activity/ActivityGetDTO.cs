using Common.DTOs.Review;
using Common.DTOs.Accessibility;
using Common.DTOs.Type;

namespace Common.DTOs.Activity;

public class ActivityGetDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Contact { get; set; }
    public string? Email { get; set; } 
    public int Views { get; set; }
    public TypeReadDTO Type { get; set; } = null!;
    public List<ReviewReadDTO> Reviews { get; set; } = new();
    public List<AccessibilityReadDTO> Accessibilities { get; set; } = new();
}
