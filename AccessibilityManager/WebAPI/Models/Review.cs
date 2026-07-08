using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Review
{
    public int Id { get; set; }

    public string? Comment { get; set; }

    public int? Grade { get; set; }

    public DateTime? Date { get; set; }

    public int? UserId { get; set; }

    public int? ActivityId { get; set; }

    public virtual Activity? Activity { get; set; }

    public virtual User? User { get; set; }
}
