namespace VehicleInsuranceAPI.Models
{
    public class CustomerBillModel
    {
        public int BillNo { get; set; }

        public int PolicyNo { get; set; }

        public string? Status { get; set; }

        public DateTime? Date { get; set; }

        public decimal? Amount { get; set; }
    }
}
