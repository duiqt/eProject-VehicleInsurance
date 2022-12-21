using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class OptionDetail
{
    public int Id { get; set; }

    public int Option1 { get; set; }

    public int Option2 { get; set; }

    public bool? IsOption1Included { get; set; }

    public bool? IsOption2Included { get; set; }

    public virtual ICollection<Estimate> Estimates { get; } = new List<Estimate>();

    public virtual Option Option1Navigation { get; set; } = null!;

    public virtual Option Option2Navigation { get; set; } = null!;
}
