namespace VehicleInsuranceClient.Areas.Employee.Models
{
    public class ClaimModel
    {
        public int Id { get; set; }
        public int ClaimNo { get; set; }
        public int PolicyNo { get; set; }
        public string PolicyType { get; set; } = null!;
        public string PlaceOfAccident { get; set; } = null!;
        public DateTime DateOfAccident { get; set; }
        public string Description { get; set; } = null!;
        public string? Status { get; set; }
        public string? Image { get; set; }
        public decimal InsuredAmount { get; set; }
        public decimal? ClaimableAmount { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? CustomerAddress { get; set; }
        public long CustomerPhone { get; set; }
        public int CustomerId { get; set; }
        public int VehicleNumber { get; set; }
        public string VehicleBodyNumber { get; set; } = null!;
        public string VehicleEngineNumber { get; set; } = null!;
        public string? VehicleWarranty { get; set; }
        public string VehicleName { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string VehicleVersion { get; set; } = null!;
    }

}
