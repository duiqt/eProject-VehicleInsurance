using DataAccess.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace VehicleInsuranceClient.Areas.Employee.Models.Dtos
{
   

    public partial class DashboardPolicy

    {
        public string PolicyType { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Annually { get; set; }

        public int Month { get; set; } 

       
    }

    public partial class DashboardDto
    {
        public int DateOfAccident { get; set; }
        public decimal? ClaimableAmount { get; set; }
    }

    public partial class DashboardOverviewDto
    {
        public decimal Income { get; set; }
        public decimal Balance { get; set; }
        public decimal Expense { get; set; }
        public decimal MonthlySell { get; set; }

        public DashboardOverviewDto(decimal _income, decimal _expense, decimal _balance, decimal _monthlySell)
        {
            Income = _income;
            Expense = _expense;
            Balance = _balance;
            MonthlySell = _monthlySell;
        }
    }

    public partial class CertificateDto
    {
        public string VehicleName { get; set; }

        public int VehicleCountYearA { get; set; }

        public int VehicleCountYearB { get; set; }

        public int VehicleCountYearC { get; set; }

        public decimal VehiclePercenYearA { get; set; }

        public decimal VehiclePercenYearB { get; set; }

        public decimal VehiclePercenYearC { get; set; }
    }
}
