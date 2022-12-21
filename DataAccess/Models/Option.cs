using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Option
{
    public int Id { get; set; }

    public string OptionType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Annually { get; set; }

    public virtual ICollection<OptionDetail> OptionDetailOption1Navigations { get; } = new List<OptionDetail>();

    public virtual ICollection<OptionDetail> OptionDetailOption2Navigations { get; } = new List<OptionDetail>();
}
