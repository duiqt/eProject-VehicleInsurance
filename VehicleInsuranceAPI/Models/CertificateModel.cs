namespace VehicleInsuranceAPI.Models
{
    public class CertificateModel
    {
        public long Id { get; set; }
        public int PolicyNo { get; set; }
        public string PolicyType { get; set; }
        public int BillNo { get; set; }
        public decimal Premium { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public long CustomerPhone { get; set; }
        public int VehicleNumber { get; set; }
        public string VehicleName { get; set; } = null!;
        public string VehicleOwnerName { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string VehicleVersion { get; set; }
        public DateTime PolicyDate { get; set; }
        public int PolicyDuration { get; set; }
        public string VehicleWarranty { get; set; } = null!;
        public string? Prove { get; set; }
    }

    public class CertificatesModel
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string PolicyType { get; set; }
        public string VehicleName { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string VehicleVersion { get; set; }
        public int PolicyNo { get; set; }
        public int PolicyDuration { get; set; }
        public DateTime PolicyDate { get; set; }
    }
}
