using Common.DTOs.Accessibility;
using Common.DTOs.Review;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.Activity;

public class ActivityUpdateDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; } 
    public string? Address { get; set; }
    public string? Contact { get; set; }
    public string? Email { get; set; }
    public string? Type { get; set; }
    public List<string>? Accessibilities { get; set; } = new List<string>();
}
