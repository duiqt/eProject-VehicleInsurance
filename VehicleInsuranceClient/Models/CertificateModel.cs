using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceClient.Models
{
    public class CertificateModel
    {
        public int Id { get; set; }
        public int EstimateNo { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public long CustomerPhone { get; set; }
        public int PolicyNo { get; set; }
        public string PolicyType { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime PolicyDate { get; set; }
        public int PolicyDuration { get; set; }
        public string VehicleName { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string VehicleVersion { get; set; }
        [Required(ErrorMessage = "Vehicle number is required")]
        [RegularExpression("^([0-9]{5,10})$", ErrorMessage = "Numbers between 5 - 10 digits")]
        public int VehicleNumber { get; set; }
        [Required(ErrorMessage = "Body number is required")]
        [RegularExpression(@"^[a-zA-Z0-9-]{6,20}$", ErrorMessage = "6-20 characters not contain the specials")]
        public string VehicleBodyNumber { get; set; } = null!;
        [Required(ErrorMessage = "Engine number is required")]
        [RegularExpression("^[a-zA-Z0-9-]{6,15}$", ErrorMessage = "6-15 characters not contain the specials")]
        public string VehicleEngineNumber { get; set; } = null!;
        public string VehicleWarranty { get; set; } = null!;
        public decimal? Premium { get; set; }
        public string? Prove { get; set; }
    }

    public class ContractModel
    {
        public Contract? Contract { get; set; } = null!;
        public EstimateViewModel? Estimation { get; set; }
    }
    public class Contract
    {
        [Required(ErrorMessage = "Vehicle owner name is required")]
        public string CustomerName { get; set; } = String.Empty!;

        [Required(ErrorMessage = "Customer address is required")]
        public string? CustomerAddress { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression("[0-9]{9,15}", ErrorMessage = "Invalid phone number")]
        public long CustomerPhone { get; set; }

        [Required(ErrorMessage = "Registration is required")]
        [RegularExpression("^([0-9]{5,10})$", ErrorMessage = "Numbers between 5 - 10 digits")]
        public string VehicleNumber { get; set; } = String.Empty!;

        [Required(ErrorMessage = "Vehicle body number is required")]
        [RegularExpression("^[a-zA-Z0-9-]{6,20}$", ErrorMessage = "6-20 characters not contain the specials")]
        public string VehicleBodyNumber { get; set; } = String.Empty!;

        [Required(ErrorMessage = "Engine number is required")]
        [RegularExpression("^[a-zA-Z0-9-]{6,15}$", ErrorMessage = "6-15 characters not contain the specials")]
        public string VehicleEngineNumber { get; set; } = null!;
        //public string? Prove { get; set; }
        [Required(ErrorMessage = "Images of prove is required")]
        public IFormFileCollection Prove { get; set; } = null!;
    }
}
