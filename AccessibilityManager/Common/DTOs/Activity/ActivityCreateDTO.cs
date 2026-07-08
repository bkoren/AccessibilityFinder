using Common.DTOs.Accessibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Activity;

public class ActivityCreateDTO
{
    [Required(ErrorMessage = "Name can't be empty!")]
    public string Name { get; set; } = null!;

    [StringLength(500, MinimumLength = 20, ErrorMessage = "Description must contain at lest 20 chars!")]
    public string Description { get; set; } = null!;
    public string? Address { get; set; }
    public string? Contact { get; set; }
    public string? Email { get; set; }

    [Required(ErrorMessage = "Type can't be empty!")]
    public string Type { get; set; } = null!;

    public int Views { get; set; } = 0;

    public List<string> Accessibilities { get; set; } = new List<string>();
}
