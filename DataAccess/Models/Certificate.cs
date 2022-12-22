using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Certificate
{
    public int Id { get; set; }

    public int PolicyNo { get; set; }

    public int EstimateId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? CustomerAddress { get; set; }

    public long CustomerPhone { get; set; }

    public int VehicleNumber { get; set; }

    public string VehicleBodyNumber { get; set; } = null!;

    public string VehicleEngineNumber { get; set; } = null!;

    public string? VehicleWarranty { get; set; }

    public string? Prove { get; set; }

    public virtual ICollection<Claim> Claims { get; } = new List<Claim>();

    public virtual ICollection<CustomerBill> CustomerBills { get; } = new List<CustomerBill>();

    public virtual Estimate Estimate { get; set; } = null!;
}
