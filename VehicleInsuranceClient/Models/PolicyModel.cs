namespace VehicleInsuranceClient.Models
{
    public class PolicyModel
    {
        public int Id { get; set; }
        public string PolicyType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Coverage { get; set; } = null!;
        public decimal Annually { get; set; }
    }
}
