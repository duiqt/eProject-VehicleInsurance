using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Claim
{
    public int Id { get; set; }

    public int ClaimNo { get; set; }

    public int PolicyNo { get; set; }

    public string PlaceOfAccident { get; set; } = null!;

    public DateTime DateOfAccident { get; set; }

    public string Description { get; set; } = null!;

    public string? Status { get; set; }

    public string? Image { get; set; }

    public decimal InsuredAmount { get; set; }

    public decimal? ClaimableAmount { get; set; }

    public virtual Certificate PolicyNoNavigation { get; set; } = null!;
}
