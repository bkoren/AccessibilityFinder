using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Review;

public class ReviewCreateDTO
{
    public int ActivityId { get; set; }
    public string? Comment {  get; set; }

    [Range(1, 5, ErrorMessage = "Grade must be between 1 and 5.")]
    public int Grade { get; set; }
}
