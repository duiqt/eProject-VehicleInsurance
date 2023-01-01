namespace VehicleInsuranceAPI.Models
{
    public class UpdateCertModel
    {
        public int PolicyNo { get; set; }
        public int VehicleNumber { get; set; }
        public string VehicleBodyNumber { get; set; } = null!;
        public string VehicleEngineNumber { get; set; } = null!;
        public DateTime PolicyDate { get; set; }
        public string VehicleWarranty { get; set; } = null!;
    }
}
