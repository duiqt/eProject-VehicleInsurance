using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Estimate
{
    public int Id { get; set; }

    public int EstimateNo { get; set; }

    public int? CustomerId { get; set; }

    public string VehicleName { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;

    public string VehicleVersion { get; set; } = null!;

    public int PolicyId { get; set; }

    public int? OptionDetailsId { get; set; }

    public DateTime PolicyDate { get; set; }

    public int PolicyDuration { get; set; }

    public decimal Premium { get; set; }

    public virtual Certificate? Certificate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual OptionDetail? OptionDetails { get; set; }

    public virtual Policy Policy { get; set; } = null!;
}
