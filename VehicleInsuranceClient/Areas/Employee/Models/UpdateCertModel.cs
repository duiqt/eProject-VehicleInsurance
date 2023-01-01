using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceClient.Areas.Employee.Models
{
    public class UpdateCertModel
    {
        public int Id { get; set; }
        [Required]
        public int PolicyNo { get; set; }

        [Required(ErrorMessage = "Vehicle number is required")]
        [RegularExpression("^([0-9]{5,10})$", ErrorMessage = "Numbers between 5 - 10 digits")]
        public string VehicleNumber { get; set; } = String.Empty;

        [Required(ErrorMessage = "Body number is required")]
        [RegularExpression(@"^[a-zA-Z0-9-]{6,20}$", ErrorMessage = "6-20 characters not contain the specials")]
        public string VehicleBodyNumber { get; set; } = String.Empty!;

        [Required(ErrorMessage = "Engine number is required")]
        [RegularExpression("^[a-zA-Z0-9-]{6,15}$", ErrorMessage = "6-15 characters not contain the specials")]
        public string VehicleEngineNumber { get; set; } = null!;

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime? PolicyDate { get; set; } = null!;
        public string VehicleWarranty { get; set; } = null!;
    }
}