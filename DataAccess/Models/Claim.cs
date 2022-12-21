using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Claim
{
    public int Id { get; set; }

    public int ClaimNo { get; set; }

    public int CertificateId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string PlaceOfAccident { get; set; } = null!;

    public string DateOfAccident { get; set; } = null!;

    public decimal InsuredAmount { get; set; }

    public decimal ClaimableAmount { get; set; }

    public virtual Certificate Certificate { get; set; } = null!;
}
