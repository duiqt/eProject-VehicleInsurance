﻿namespace VehicleInsuranceAPI.Models
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
        public DateTime PolicyDate { get; set; }
        public int PolicyDuration { get; set; }
        public string VehicleName { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string VehicleVersion { get; set; }
        public int VehicleNumber { get; set; }
        public string VehicleBodyNumber { get; set; } = null!;
        public string VehicleEngineNumber { get; set; } = null!;
        public string VehicleWarranty { get; set; } = null!;
        public decimal? Amount { get; set; }
        public string? Prove { get; set; }
        public int BillNo { get; set; }
        public DateTime? Date { get; set; }
        public string? Status { get; set; }
    }

    public class CertificateAdminModel
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public long CustomerPhone { get; set; }
        public int PolicyNo { get; set; }
        public string PolicyType { get; set; }
        public DateTime PolicyDate { get; set; }
        public int PolicyDuration { get; set; }
        public string VehicleName { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string VehicleVersion { get; set; }
        public int VehicleNumber { get; set; }
        public string VehicleBodyNumber { get; set; } = null!;
        public string VehicleEngineNumber { get; set; } = null!;
        public string VehicleWarranty { get; set; } = null!;
        public decimal? Amount { get; set; }
        public string? Prove { get; set; }
        public string? Status { get; set; }
    }
}
