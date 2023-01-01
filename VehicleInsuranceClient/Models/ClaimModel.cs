namespace VehicleInsuranceClient.Models
{
    public class ClaimViewModel 
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
        public decimal? ClaimableAmount { get; set; } = 0;
        public string SelectedStatus { get; set; }
    }
}
