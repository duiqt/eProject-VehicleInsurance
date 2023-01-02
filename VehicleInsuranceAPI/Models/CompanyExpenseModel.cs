using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceAPI.Models
{
    public class CompanyExpenseModel
    {
        public int Id { get; set; }

        public string DateOfExpense { get; set; } = null!;

        public string TypeOfExpense { get; set; } = null!;

        public decimal AmountOfExpense { get; set; }
    }

    public class CompanyExpenseMonthlyModel
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfExpense { get; set; }
        public string TypeOfExpense { get; set; } = null!;
        public decimal? AmountOfExpense { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfAccident { get; set; }
        public string PolicyType { get; set; }
        public int ClaimNo { get; set; }
        public decimal? ClaimableAmount { get; set; }
        public string? Status { get; set; }
    }
}
