using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Vehicle
{
    public int Id { get; set; }

    public string VehicleName { get; set; } = null!;

    public string VehicleOwnerName { get; set; } = null!;

    public string VehicleModel { get; set; } = null!;

    public string VehicleVersion { get; set; } = null!;

    public decimal VehicleRate { get; set; }

    public string VehicleBodyNumber { get; set; } = null!;

    public string VehicleEngineNumber { get; set; } = null!;

    public int VehicleNumber { get; set; }
}
