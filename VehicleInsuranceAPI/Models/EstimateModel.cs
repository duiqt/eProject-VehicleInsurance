using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceAPI.Models
{
    public class EstimateModel
    {
        public int PolicyId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleVersion { get; set; }
        public decimal VehicleRate { get; set; }
    }
    public class EstimateApiModel
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
        public int PolicyId { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime? PolicyDate { get; set; } = null!;
        [DataType(DataType.Date)]
        public DateTime? EstimateDate { get; set; } = DateTime.Now!;
        public int PolicyDuration { get; set; } = 12;

        public decimal? Premium { get; set; }
        [Required(ErrorMessage = "Vehicle Rate is required")]
        public decimal VehicleRate { get; set; }
    }
}