using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceClient.Models
{
    public class PoliciesViewModel
    {
        public int PolicyId { get; set; }
        public string PolicyType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Coverage { get; set; } = null!;
        public decimal Annually { get; set; }
    }
    public class VehicleViewModel
    {
        public string VehicleName { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string VehicleVersion { get; set; } = null!;
    }
    public class EstimateViewModel
    {
        public string PolicyType { get; set; }
        public string VehicleName { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleVersion { get; set; }
        public decimal VehicleRate { get; set; }
    }

    public class EstimateClientViewModel
    {
        public int? EstimateNo { get; set; } = 0;

        public int? CustomerId { get; set; }
        [Required(ErrorMessage = "Vehicle Name is required")]
        public string VehicleName { get; set; } = null!;
        [Required(ErrorMessage = "Vehicle Model is required")]
        public string VehicleModel { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vehicle Version is required")]
        public string VehicleVersion { get; set; } = string.Empty;
        [Required]
        public string PolicyType { get; set; } = string.Empty;
        public int? OptionDetailsId { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.DateTime)]
        public DateTime? PolicyDate { get; set; }

        public int? PolicyDuration { get; set; }

        public decimal? Premium { get; set; }
        [Required(ErrorMessage = "Vehicle Rate is required")]
        public decimal VehicleRate { get; set; }
    }
}
