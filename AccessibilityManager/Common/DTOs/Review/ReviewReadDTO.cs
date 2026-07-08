using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.Review;

public class ReviewReadDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Activity { get; set; } = null!;
    public string Date { get; set; } = null!;
    public string Comment { get; set; } = null!;
    public int Grade { get; set; }

}
