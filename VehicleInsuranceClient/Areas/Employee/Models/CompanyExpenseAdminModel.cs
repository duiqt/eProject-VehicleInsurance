using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceClient.Areas.Employee.Models
{
    public class CompanyExpenseAdminModel
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfExpense { get; set; }
        public string TypeOfExpense { get; set; } = null!;
        public decimal AmountOfExpense { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfAccident { get; set; }
        public string PolicyType { get; set; }
        public int ClaimNo { get; set; }
        public decimal ClaimableAmount { get; set; }
        public string? Status { get; set; }
    }

    public class ClaimDetailCompanyExpenseAdminModel
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfExpense { get; set; }
        public string TypeOfExpense { get; set; } = null!;
        public decimal AmountOfExpense { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfAccident { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public long CustomerPhone { get; set; }
        public int PolicyNo { get; set; }
        public string PolicyType { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PolicyDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }
        public int PolicyDuration { get; set; }
        public string VehicleName { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleVersion { get; set; }
        public string VehicleBodyNumber { get; set; } = null!;
        public string VehicleEngineNumber { get; set; } = null!;
        public int VehicleNumber { get; set; }
        public string VehicleWarranty { get; set; }
        public string? Prove { get; set; }
        public string? Image { get; set; }
        public decimal? Amount { get; set; }

        public int ClaimNo { get; set; }

        public int CertificateId { get; set; }

        public string PlaceOfAccident { get; set; } = null!;
        public decimal InsuredAmount { get; set; }
        public decimal ClaimableAmount { get; set; }
        public string? Status { get; set; }
    }
}
