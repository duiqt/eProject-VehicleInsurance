using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class CustomerBill
{
    public int Id { get; set; }

    public int BillNo { get; set; }

    public int CertificateId { get; set; }

    public string? CustomerAddProve { get; set; }

    public DateTime? Date { get; set; }

    public decimal? Amount { get; set; }

    public virtual Certificate Certificate { get; set; } = null!;
}
