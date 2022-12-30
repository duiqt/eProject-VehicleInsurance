using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string CustomerEmail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string? CustomerAddress { get; set; }

    public long CustomerPhone { get; set; }

    public virtual ICollection<Certificate> Certificates { get; } = new List<Certificate>();
}
