using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Type
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Activity> Activities { get; set; } = [];
}
