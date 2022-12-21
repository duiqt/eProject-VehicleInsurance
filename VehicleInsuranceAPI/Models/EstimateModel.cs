namespace VehicleInsuranceAPI.Models
{
    public class EstimateModel
    {
        public string PolicyType { get; set; }
        public string VehicleName { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleVersion { get; set; }
        public decimal VehicleRate { get; set; }
    }
}