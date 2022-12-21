using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Policy
{
    public int Id { get; set; }

    public string PolicyType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Coverage { get; set; } = null!;

    public decimal Annually { get; set; }

    public virtual ICollection<Estimate> Estimates { get; } = new List<Estimate>();
}
