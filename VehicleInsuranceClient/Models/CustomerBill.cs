using System;
using System.Collections.Generic;

namespace VehicleInsuranceClient.Models;

public partial class CustomerBill
{
    public int Id { get; set; }

    public int BillNo { get; set; }

    public int PolicyNo { get; set; }

    public string? Status { get; set; }

    public DateTime? Date { get; set; }

    public decimal? Amount { get; set; }

}
