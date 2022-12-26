using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Estimate
{
    public int Id { get; set; }

    public int EstimateNo { get; set; }

    public string VehicleName { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;

    public string VehicleVersion { get; set; } = null!;

    public int PolicyId { get; set; }

    public DateTime EstimateDate { get; set; }

    public DateTime PolicyDate { get; set; }

    public int PolicyDuration { get; set; }

    public decimal Premium { get; set; }

    public virtual ICollection<Certificate> Certificates { get; } = new List<Certificate>();

    public virtual Policy Policy { get; set; } = null!;
}
