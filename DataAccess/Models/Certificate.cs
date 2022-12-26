using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Certificate
{
    public int Id { get; set; }

    public int PolicyNo { get; set; }

    public int EstimateNo { get; set; }

    public int CustomerId { get; set; }

    public int VehicleNumber { get; set; }

    public string VehicleBodyNumber { get; set; } = null!;

    public string VehicleEngineNumber { get; set; } = null!;

    public string? VehicleWarranty { get; set; }

    public string? Prove { get; set; }

    public virtual ICollection<Claim> Claims { get; } = new List<Claim>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<CustomerBill> CustomerBills { get; } = new List<CustomerBill>();

    public virtual Estimate EstimateNoNavigation { get; set; } = null!;
}
